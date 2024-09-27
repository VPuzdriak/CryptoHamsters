using System.Linq.Expressions;

using Humanizer;

using Marten;

namespace CryptoHamsters.Shared.Marten;

public static class QuerySessionExtensions
{
    public static async Task<T?> LoadAndAggregate<T>(
        this IQuerySession querySession,
        Guid streamId,
        CancellationToken cancellationToken) where T : class, IAggregate
    {
        var aggregate = await querySession.LoadAsync<T>(streamId, cancellationToken);
        var aggregateVersion = aggregate?.Version ?? 0;
        return await querySession.Events.AggregateStreamAsync(
            streamId,
            fromVersion: aggregateVersion,
            state: aggregate,
            token: cancellationToken);
    }

    public static async Task<T?> QueryAndAggregateOneAsync<T>(
        this IQuerySession querySession,
        Expression<Func<T, bool>> predicate,
        long untilSequence,
        CancellationToken cancellationToken) where T : IAggregate
    {
        if (!await querySession.WaitForSyncAsync<T>(untilSequence))
        {
            return default;
        }

        return await querySession.Query<T>().Where(predicate).FirstOrDefaultAsync(cancellationToken);
    }

    public static async Task<IReadOnlyList<T>> QueryAndAggregateAsync<T>(
        this IQuerySession querySession,
        Expression<Func<T, bool>> predicate,
        long untilSequence,
        CancellationToken cancellationToken) where T : IAggregate
    {
        if (!await querySession.WaitForSyncAsync<T>(untilSequence))
        {
            return [];
        }

        return await querySession.Query<T>().Where(predicate).ToListAsync(cancellationToken);
    }

    private static async Task<bool> WaitForSyncAsync<T>(this IQuerySession querySession,
        long sequence,
        int delayInMilliseconds = 100,
        int maxAttempts = 3) where T : IAggregate
    {
        var checkpointName = GetCheckpointName<T>();

        var checkpointValue = querySession.Query<long>(
                "select last_seq_id from events.mt_event_progression where name = ?", checkpointName)
            .FirstOrDefault();

        int attempts = 0;

        while (checkpointValue < sequence && attempts <= maxAttempts)
        {
            await Task.Delay(delayInMilliseconds);

            checkpointValue = querySession.Query<long>(
                    "select last_seq_id from events.mt_event_progression where name = ?", checkpointName)
                .FirstOrDefault();

            attempts++;
        }

        return checkpointValue >= sequence;
    }

    private static string GetCheckpointName<T>() where T : IAggregate =>
        typeof(T).Name.ToLower().Kebaberize().Pluralize() + ":All";
}