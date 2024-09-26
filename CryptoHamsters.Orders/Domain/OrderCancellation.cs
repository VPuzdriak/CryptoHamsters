namespace CryptoHamsters.Orders.Domain;

public sealed record OrderCancellation(string Reason, DateTime CancelledAtUtc);