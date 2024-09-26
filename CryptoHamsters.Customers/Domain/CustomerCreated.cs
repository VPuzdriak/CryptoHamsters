namespace CryptoHamsters.Customers.Domain;

public record CustomerCreated(Guid Id, string Name, DateTime CreatedAtUtc);