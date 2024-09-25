namespace CryptoHamsters.CryptoPairs.Domain;

public record CryptoPairListed(
    Guid Id,
    string Ticker,
    string BaseAsset,
    string QuoteAsset,
    DateTime ListedAtUtc,
    decimal Price);