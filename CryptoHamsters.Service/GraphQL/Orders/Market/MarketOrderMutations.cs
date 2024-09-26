using CryptoHamsters.Orders.MarketOrders.Place;

using MediatR;

namespace CryptoHamsters.Service.GraphQL.Orders.Market;

[MutationType]
public static class MarketOrderMutations
{
    public static async Task<MarketOrderPayload> PlaceMarketOrder(
        PlaceMarketOrderInput input,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var marketOrder = await
            mediator.Send(
                new PlaceMarketOrder(
                    input.WalletId,
                    input.CryptoPairId,
                    input.QuoteAssetAmount),
                cancellationToken);

        return new MarketOrderPayload(
            marketOrder.Id,
            marketOrder.WalletId,
            marketOrder.CryptoPairId,
            marketOrder.Status,
            marketOrder.PlacedAtUtc,
            marketOrder.QuoteAssetAmount,
            null,
            null,
            null,
            null,
            null);
    }
}