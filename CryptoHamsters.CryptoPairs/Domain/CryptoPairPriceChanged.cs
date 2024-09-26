namespace CryptoHamsters.CryptoPairs.Domain;

public record CryptoPairPriceChanged(Guid PairId, decimal PriceChange, DateTime PriceUpdatedAtUtc);