using CryptoHamsters.Orders.MarketOrders.Get;

using MediatR;

namespace CryptoHamsters.Service.GraphQL.Orders.Market;

[QueryType]
public static class MarketOrderQueries
{
    public static async Task<IReadOnlyList<MarketOrderPayload>> GetMarketOrders(
        Guid walletId,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var marketOrders = await mediator.Send(new GetMarketOrders(walletId), cancellationToken);
        return marketOrders.Select(mo =>
                new MarketOrderPayload(
                    mo.Id,
                    mo.WalletId,
                    mo.CryptoPairId,
                    mo.Status,
                    mo.PlacedAtUtc,
                    mo.QuoteAssetAmount,
                    mo.Fulfilment?.FulfilledAmount,
                    mo.Fulfilment?.FulfilledPrice,
                    mo.Fulfilment?.FulfilledAtUtc,
                    mo.Cancellation?.CancelledAtUtc,
                    mo.Cancellation?.Reason))
            .ToList();
    }
}