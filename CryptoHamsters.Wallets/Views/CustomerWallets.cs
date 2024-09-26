using CryptoHamsters.Customers.Domain;
using CryptoHamsters.Wallets.Domain;

using Marten.Events.Projections;

namespace CryptoHamsters.Wallets.Views;

public sealed record CustomerWallets(Guid Id, string CustomerName, HashSet<CustomerWalletSpecs> Wallets);

public sealed record CustomerWalletSpecs(Guid Id, WalletType Type, DateTime CreatedAtUtc);

public sealed class CustomerWalletsProjection : MultiStreamProjection<CustomerWallets, Guid>
{
    public CustomerWalletsProjection()
    {
        Identity<CustomerCreated>(c => c.Id);
        Identity<WalletCreated>(w => w.CustomerId);

        ProjectionName = "customer_wallets";
    }

    public static CustomerWallets Create(CustomerCreated customerCreated) =>
        new(customerCreated.Id, customerCreated.Name, []);

    public CustomerWallets Apply(CustomerWallets customerWallets, WalletCreated walletCreated) =>
        customerWallets with
        {
            Wallets =
            [
                ..customerWallets.Wallets,
                new CustomerWalletSpecs(walletCreated.Id, walletCreated.Type, walletCreated.CreatedAtUtc)
            ]
        };
}