namespace CryptoHamsters.Orders.Domain;

public record MarketOrderCancelled(
    Guid Id,
    string Reason,
    DateTime CancelledAtUtc);