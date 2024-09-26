namespace CryptoHamsters.Orders.Domain;

public record MarketOrderFulfilled(
    Guid Id,
    decimal Amount,
    decimal Price,
    DateTime FulfilledAtUtc);