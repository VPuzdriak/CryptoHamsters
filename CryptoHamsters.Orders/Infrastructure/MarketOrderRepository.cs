using CryptoHamsters.Orders.Domain;

using Marten;

namespace CryptoHamsters.Orders.Infrastructure;

public interface IMarketOrderRepository
{
    Task<IReadOnlyList<MarketOrder>> GetAsync(Guid walletId, CancellationToken cancellationToken);
    Task<IReadOnlyList<MarketOrder>> GetPlacedOrdersAsync(CancellationToken cancellationToken, int first = 10);
    Task CreateAsync(Guid id, MarketOrderPlaced marketOrderPlaced, CancellationToken cancellationToken);
    Task UpdateWithAsync<T>(Guid id, T @event, CancellationToken cancellationToken) where T : class;
}

internal sealed class MarketOrderRepository(IDocumentSession documentSession) : IMarketOrderRepository
{
    public Task<IReadOnlyList<MarketOrder>> GetAsync(Guid walletId, CancellationToken cancellationToken) =>
        documentSession.Query<MarketOrder>().Where(mo => mo.WalletId == walletId).ToListAsync(cancellationToken);

    public Task<IReadOnlyList<MarketOrder>> GetPlacedOrdersAsync(CancellationToken cancellationToken, int first = 10) =>
        documentSession.Query<MarketOrder>()
            .Where(mo => mo.Status == OrderStatus.Placed)
            .OrderBy(mo => mo.PlacedAtUtc)
            .Take(first)
            .ToListAsync(cancellationToken);

    public Task CreateAsync(Guid id, MarketOrderPlaced marketOrderPlaced, CancellationToken cancellationToken)
    {
        documentSession.Events.StartStream<MarketOrder>(id, marketOrderPlaced);
        return documentSession.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateWithAsync<T>(Guid id, T @event, CancellationToken cancellationToken) where T : class
    {
        documentSession.Events.Append(id, @event);
        return documentSession.SaveChangesAsync(cancellationToken);
    }
}