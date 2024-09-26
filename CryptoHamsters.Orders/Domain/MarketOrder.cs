namespace CryptoHamsters.Orders.Domain;

public sealed class MarketOrder
{
    public Guid Id { get; init; }
    public Guid WalletId { get; init; }
    public Guid CryptoPairId { get; init; }
    public decimal QuoteAssetAmount { get; init; }
    public DateTime PlacedAtUtc { get; init; }
    public long Version { get; set; }
    public OrderStatus Status { get; set; }
    public OrderFulfilment? Fulfilment { get; set; }
    public OrderCancellation? Cancellation { get; set; }

    private MarketOrder()
    {
        Status = OrderStatus.Placed;
        Version = -1;
    }

    private MarketOrder(Guid id, Guid walletId, Guid cryptoPairId, decimal quoteAssetAmount, DateTime placedAtUtc)
    {
        Id = id;
        WalletId = walletId;
        CryptoPairId = cryptoPairId;
        QuoteAssetAmount = quoteAssetAmount;
        PlacedAtUtc = placedAtUtc;
        Status = OrderStatus.Placed;
        Version = 1;
    }

    public static MarketOrder Create(MarketOrderPlaced @event) =>
        new(
            @event.Id,
            @event.WalletId,
            @event.CryptoPairId,
            @event.QuoteAssetAmount,
            @event.PlacedAtUtc);

    public void Apply(MarketOrderFulfilled @event)
    {
        Fulfilment = new OrderFulfilment(@event.Amount, @event.Price, @event.FulfilledAtUtc);
        Status = OrderStatus.Fulfilled;
        Version++;
    }

    public void Apply(MarketOrderCancelled @event)
    {
        Cancellation = new OrderCancellation(@event.Reason, @event.CancelledAtUtc);
        Status = OrderStatus.Cancelled;
        Version++;
    }
}