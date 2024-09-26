using CryptoHamsters.Orders.Domain;

namespace CryptoHamsters.Service.GraphQL.Orders.Market;

public record MarketOrderPayload(
    Guid OrderId,
    Guid WalletId,
    Guid CryptoPairId,
    OrderStatus Status,
    DateTime PlacedAtUtc,
    decimal QuoteAssetAmount,
    decimal? FulfilledAmount,
    decimal? FulfillmentPrice,
    DateTime? FulfilledAtUtc,
    DateTime? CancelledAtUtc,
    string? CancellationReason);