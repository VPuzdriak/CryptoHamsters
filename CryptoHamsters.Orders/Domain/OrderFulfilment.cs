namespace CryptoHamsters.Orders.Domain;

public sealed record OrderFulfilment(decimal FulfilledAmount, decimal FulfilledPrice, DateTime FulfilledAtUtc);