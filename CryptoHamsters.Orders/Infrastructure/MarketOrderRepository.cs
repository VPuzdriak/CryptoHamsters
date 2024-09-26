using CryptoHamsters.Orders.Domain;

using Marten;

namespace CryptoHamsters.Orders.Infrastructure;

public interface IMarketOrderRepository
{
    Task<IReadOnlyList<MarketOrder>> GetAsync(Guid walletId, CancellationToken cancellationToken);
    Task CreateAsync(Guid id, MarketOrderPlaced marketOrderPlaced, CancellationToken cancellationToken);
}

internal sealed class MarketOrderRepository(IDocumentSession documentSession) : IMarketOrderRepository
{
    public Task<IReadOnlyList<MarketOrder>> GetAsync(Guid walletId, CancellationToken cancellationToken) =>
        documentSession.Query<MarketOrder>().Where(mo => mo.WalletId == walletId).ToListAsync(cancellationToken);

    public Task CreateAsync(Guid id, MarketOrderPlaced marketOrderPlaced, CancellationToken cancellationToken)
    {
        documentSession.Events.StartStream<MarketOrder>(id, marketOrderPlaced);
        return documentSession.SaveChangesAsync(cancellationToken);
    }
}