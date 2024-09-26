using CryptoHamsters.CryptoPairs.Domain;

using Marten;

namespace CryptoHamsters.CryptoPairs.Infrastructure;

public interface ICryptoPairsRepository
{
    Task<bool> PairExistsAsync(Guid id, CancellationToken cancellationToken);

    Task CreateAsync(
        Guid pairId,
        CryptoPairListed cryptoPairListed,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<CryptoPair>> GetCryptoPairsAsync(string quoteAsset, CancellationToken cancellationToken);
    Task UpdateWithAsync<T>(Guid pairId, T @event, CancellationToken cancellationToken) where T : class;
}

internal sealed class CryptoPairsRepository(IDocumentSession documentSession) : ICryptoPairsRepository
{
    public async Task<bool> PairExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        var cryptoPair = await documentSession.Events.AggregateStreamAsync<CryptoPair>(id, token: cancellationToken);
        return cryptoPair != null;
    }

    public async Task CreateAsync(
        Guid pairId,
        CryptoPairListed cryptoPairListed,
        CancellationToken cancellationToken)
    {
        documentSession.Events.StartStream<CryptoPair>(pairId, cryptoPairListed);
        await documentSession.SaveChangesAsync(cancellationToken);
    }

    public Task<IReadOnlyList<CryptoPair>> GetCryptoPairsAsync(
        string quoteAsset,
        CancellationToken cancellationToken) =>
        documentSession.Query<CryptoPair>()
            .Where(pair => pair.QuoteAsset == quoteAsset)
            .OrderByDescending(pair => pair.Price)
            .ToListAsync(cancellationToken);

    public Task UpdateWithAsync<T>(Guid pairId, T @event, CancellationToken cancellationToken) where T : class
    {
        documentSession.Events.Append(pairId, @event);
        return documentSession.SaveChangesAsync(cancellationToken);
    }
}