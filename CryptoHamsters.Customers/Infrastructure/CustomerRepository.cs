using CryptoHamsters.Customers.Domain;

using Marten;

namespace CryptoHamsters.Customers.Infrastructure;

public interface ICustomerRepository
{
    Task<bool> ExistsAsync(Guid customerId, CancellationToken cancellationToken);
    Task CreateAsync(Guid customerId, CustomerCreated customerCreated, CancellationToken cancellationToken);
}

internal sealed class CustomerRepository(IDocumentSession documentSession) : ICustomerRepository
{
    public async Task<bool> ExistsAsync(Guid customerId, CancellationToken cancellationToken)
    {
        var customer =
            await documentSession.Events.AggregateStreamAsync<Customer>(customerId, token: cancellationToken);

        return customer is not null;
    }

    public Task CreateAsync(Guid customerId, CustomerCreated customerCreated, CancellationToken cancellationToken)
    {
        documentSession.Events.StartStream<Customer>(customerId, customerCreated);
        return documentSession.SaveChangesAsync(cancellationToken);
    }
}