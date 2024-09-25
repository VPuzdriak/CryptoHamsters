namespace CryptoHamsters.Service.GraphQL.CryptoPairs;

public record CryptoPairPayload(
    Guid Id,
    string Ticker,
    DateTime ListedAtUtc,
    decimal Price,
    DateTime PriceUpdatedAtUtc);