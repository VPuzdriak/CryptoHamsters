using CryptoHamsters.CryptoPairs.Domain;
using CryptoHamsters.CryptoPairs.Listing;

using MediatR;

namespace CryptoHamsters.Service.GraphQL.CryptoPairs.Observe;

[SubscriptionType]
public static class CryptoPairPriceChangeSubscription
{
    [Subscribe]
    [Topic(nameof(CryptoPairPriceChanged))]
    public static async Task<IReadOnlyList<CryptoPairPayload>> ActualMarketPrices(
        [EventMessage] CryptoPairPriceChanged _,
        IMediator mediator)
    {
        var cryptoPairs = await mediator.Send(new GetCryptoPairs(StableCoins.USDT), CancellationToken.None);
        return cryptoPairs.Select(cp =>
                new CryptoPairPayload(
                    cp.Id,
                    cp.Ticker,
                    cp.ListedAtUtc,
                    cp.Price,
                    cp.PriceUpdatedAtUtc))
            .ToList();
    }
}