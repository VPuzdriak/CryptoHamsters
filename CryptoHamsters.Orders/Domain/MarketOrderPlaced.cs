namespace CryptoHamsters.Orders.Domain;

public record MarketOrderPlaced(
    Guid Id,
    Guid WalletId,
    Guid CryptoPairId,
    decimal QuoteAssetAmount,
    DateTime PlacedAtUtc);