namespace CryptoHamsters.Customers.Domain;

public sealed class Customer
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public DateTime CreatedAtUtc { get; init; }

    private Customer()
    {
        Id = Guid.Empty;
        Name = string.Empty;
        CreatedAtUtc = DateTime.MinValue;
    }

    private Customer(Guid id, string name, DateTime createdAtUtc)
    {
        Id = id;
        Name = name;
        CreatedAtUtc = createdAtUtc;
    }

    public static Customer Create(CustomerCreated @event) =>
        new(@event.Id, @event.Name, @event.CreatedAtUtc);
}