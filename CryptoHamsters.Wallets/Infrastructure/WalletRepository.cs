using CryptoHamsters.Wallets.Domain;

using Marten;

namespace CryptoHamsters.Wallets.Infrastructure;

public interface IWalletRepository
{
    Task CreateAsync(Guid walletId, WalletCreated walletCreated, CancellationToken cancellationToken);

    Task CreateAsync(
        IEnumerable<Guid> walletsId,
        IEnumerable<WalletCreated> walletsCreated,
        CancellationToken cancellationToken);

    Task UpdateWithAsync<T>(Guid walletId, T @event, CancellationToken cancellationToken) where T : class;
    Task UpdateWithAsync(Guid walletId, CancellationToken cancellationToken, params object[] events);
    Task<Wallet?> GetAsync(Guid walletId, CancellationToken cancellationToken);
}

internal sealed class WalletRepository(IDocumentSession documentSession) : IWalletRepository
{
    public async Task CreateAsync(Guid walletId, WalletCreated walletCreated, CancellationToken cancellationToken)
    {
        documentSession.Events.StartStream<Wallet>(walletId, walletCreated);
        await documentSession.SaveChangesAsync(cancellationToken);
    }

    public Task CreateAsync(IEnumerable<Guid> walletsId, IEnumerable<WalletCreated> walletsCreated,
        CancellationToken cancellationToken)
    {
        var streams = walletsId.Zip(walletsCreated, (id, created) => (id, created));

        foreach (var stream in streams)
        {
            documentSession.Events.StartStream<Wallet>(stream.id, stream.created);
        }

        return documentSession.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateWithAsync<T>(Guid walletId, T @event, CancellationToken cancellationToken)
        where T : class
    {
        documentSession.Events.Append(walletId, @event);
        await documentSession.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateWithAsync(Guid walletId, CancellationToken cancellationToken, params object[] events)
    {
        documentSession.Events.Append(walletId, events);
        return documentSession.SaveChangesAsync(cancellationToken);
    }

    public Task<Wallet?> GetAsync(Guid walletId, CancellationToken cancellationToken)
    {
        return documentSession.LoadAsync<Wallet>(walletId, cancellationToken);
    }
}