using CryptoHamsters.CryptoPairs.Domain;

using HotChocolate.Subscriptions;

using Marten;
using Marten.Events;
using Marten.Events.Daemon;
using Marten.Events.Daemon.Internals;
using Marten.Subscriptions;

namespace CryptoHamsters.CryptoPairs.Infrastructure;

public sealed class PriceChangedSubscription(ITopicEventSender sender) : SubscriptionBase
{
    public override async Task<IChangeListener> ProcessEventsAsync(
        EventRange page,
        ISubscriptionController controller,
        IDocumentOperations operations,
        CancellationToken cancellationToken)
    {
        foreach (var @event in page.Events.OfType<IEvent<CryptoPairPriceChanged>>())
        {
            var priceChanged = @event.Data;
            await sender.SendAsync(nameof(CryptoPairPriceChanged), priceChanged, cancellationToken);
        }

        return NullChangeListener.Instance;
    }
}