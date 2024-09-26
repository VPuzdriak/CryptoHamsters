using CryptoHamsters.Customers.Domain;
using CryptoHamsters.Wallets.Domain;

using HotChocolate.Utilities;

using Marten;
using Marten.Events;
using Marten.Events.Daemon;
using Marten.Events.Daemon.Internals;
using Marten.Subscriptions;

namespace CryptoHamsters.Wallets.Infrastructure;

public sealed class CustomerCreatedSubscription(IWalletRepository walletRepository) : SubscriptionBase
{
    public override async Task<IChangeListener> ProcessEventsAsync(
        EventRange page,
        ISubscriptionController controller,
        IDocumentOperations operations,
        CancellationToken cancellationToken)
    {
        foreach (var @event in page.Events.OfType<IEvent<CustomerCreated>>())
        {
            CustomerCreated customerCreated = @event.Data;

            var spotWalletCreated = new WalletCreated(
                Guid.NewGuid(),
                customerCreated.Id,
                WalletType.Spot,
                DateTime.UtcNow,
                @event.Id);

            var futuresWalletCreated = new WalletCreated(
                Guid.NewGuid(),
                customerCreated.Id,
                WalletType.Futures,
                DateTime.UtcNow,
                @event.Id);

            await walletRepository.CreateAsync(
                [spotWalletCreated.Id, futuresWalletCreated.Id],
                [spotWalletCreated, futuresWalletCreated],
                cancellationToken);
        }

        return NullChangeListener.Instance;
    }
}