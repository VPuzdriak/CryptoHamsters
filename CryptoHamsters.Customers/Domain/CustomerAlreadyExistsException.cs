namespace CryptoHamsters.Customers.Domain;

public sealed class CustomerAlreadyExistsException(Guid Id) : Exception("Customer already exists.");