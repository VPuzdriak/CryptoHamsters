using CryptoHamsters.Customers.Domain;
using CryptoHamsters.Shared.Marten;
using CryptoHamsters.Wallets.Domain;

using Marten;
using Marten.Events;
using Marten.Events.Projections;
using Marten.Patching;

namespace CryptoHamsters.Wallets.Views;

// public sealed record CustomerWallets(Guid Id, string CustomerName, HashSet<CustomerWalletSpecs> Wallets);

public sealed record CustomerWalletSpecs(Guid Id, WalletType Type, List<WalletAsset> Assets, DateTime CreatedAtUtc);

public sealed class CustomerWalletsProjection : IProjection
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; }
    public List<CustomerWalletSpecs> Wallets { get; set; }

    public void Apply(IDocumentOperations operations, IReadOnlyList<StreamAction> streams)
    {
    }

    public async Task ApplyAsync(IDocumentOperations operations, IReadOnlyList<StreamAction> streams,
        CancellationToken cancellation)
    {
        var events = streams.SelectMany(x => x.Events).OrderBy(s => s.Sequence);

        foreach (var @event in events)
        {
            switch (@event.Data)
            {
                case CustomerCreated customerCreated:
                    operations.Store(new CustomerWalletsProjection
                    {
                        Id = customerCreated.Id, CustomerName = customerCreated.Name, Wallets = []
                    });
                    break;
                case WalletCreated walletCreated:
                    {
                        var wallets = await operations
                            .QueryAndAggregateAsync<Wallet>(
                                w => w.CustomerId == walletCreated.CustomerId,
                                @event.Sequence,
                                cancellation);

                        var walletsSpecs = wallets
                            .Select(w => new CustomerWalletSpecs(w.Id, w.Type, w.Assets.ToList(), w.CreatedAtUtc))
                            .ToList();

                        operations.Patch<CustomerWalletsProjection>(walletCreated.CustomerId)
                            .Set(p => p.Wallets, walletsSpecs);
                        break;
                    }
                case WalletToppedUp walletToppedUp:
                    {
                        var wallet = await operations.QueryAndAggregateOneAsync<Wallet>(
                            w => w.Id == walletToppedUp.WalletId,
                            @event.Sequence,
                            cancellation);

                        if (wallet is null)
                        {
                            continue;
                        }

                        var customerId = wallet.CustomerId;

                        var wallets = await operations.QueryAndAggregateAsync<Wallet>(
                            w => w.CustomerId == customerId,
                            @event.Sequence,
                            cancellation);

                        if (!wallets.Any())
                        {
                            continue;
                        }

                        var walletsSpecs = wallets
                            .Select(w => new CustomerWalletSpecs(w.Id, w.Type, w.Assets.ToList(), w.CreatedAtUtc))
                            .ToList();

                        operations.Patch<CustomerWalletsProjection>(customerId)
                            .Set(p => p.Wallets, walletsSpecs);
                        break;
                    }
                case WalletAssetWithdrawn walletAssetWithdrawn:
                    {
                        var wallet = await operations.QueryAndAggregateOneAsync<Wallet>(
                            w => w.Id == walletAssetWithdrawn.WalletId,
                            @event.Sequence,
                            cancellation);

                        if (wallet is null)
                        {
                            continue;
                        }

                        var customerId = wallet.CustomerId;

                        var wallets = await operations.QueryAndAggregateAsync<Wallet>(
                            w => w.CustomerId == customerId,
                            @event.Sequence,
                            cancellation);

                        if (!wallets.Any())
                        {
                            continue;
                        }

                        var walletsSpecs = wallets
                            .Select(w => new CustomerWalletSpecs(w.Id, w.Type, w.Assets.ToList(), w.CreatedAtUtc))
                            .ToList();

                        operations.Patch<CustomerWalletsProjection>(customerId)
                            .Set(p => p.Wallets, walletsSpecs);
                        break;
                    }
            }
        }
    }
}

// public sealed class CustomerWalletsProjection : MultiStreamProjection<CustomerWallets, Guid>
// {
//     public CustomerWalletsProjection()
//     {
//         Identity<CustomerCreated>(c => c.Id);
//         Identity<WalletCreated>(w => w.CustomerId);
//
//         ProjectEventAsync<WalletToppedUp>(async (session, projection, @event) =>
//         {
//             var wallet = await session.Events.AggregateStreamAsync<Wallet>(@event.WalletId);
//             if (wallet is null)
//             {
//                 return;
//             }
//
//             var walletSpecs = projection.Wallets.First(w => w.Id == wallet.Id);
//             var walletAsset = walletSpecs.Assets.FirstOrDefault(a => a.Name == @event.Asset.Name);
//
//             if (walletAsset is not null)
//             {
//                 walletSpecs.Assets.Remove(walletAsset);
//             }
//             else
//             {
//                 walletAsset = new WalletAsset(@event.Asset.Name, 0);
//             }
//
//             walletAsset = walletAsset with { Amount = walletAsset.Amount + @event.Asset.Amount };
//             walletSpecs.Assets.Add(walletAsset);
//         });
//
//         ProjectionName = "customer_wallets";
//     }
//
//     public static CustomerWallets Create(CustomerCreated customerCreated) =>
//         new(customerCreated.Id, customerCreated.Name, []);
//
//     public CustomerWallets Apply(CustomerWallets customerWallets, WalletCreated walletCreated) =>
//         customerWallets with
//         {
//             Wallets =
//             [
//                 ..customerWallets.Wallets,
//                 new CustomerWalletSpecs(walletCreated.Id, walletCreated.Type, [], walletCreated.CreatedAtUtc)
//             ]
//         };
// }