using CryptoHamsters.Customers.Domain;
using CryptoHamsters.Customers.Infrastructure;

using MediatR;

namespace CryptoHamsters.Customers.Create;

public record CreateCustomer(Guid Id, string Name) : IRequest<Customer>;

internal sealed class CreateCustomerHandler(ICustomerRepository customerRepository)
    : IRequestHandler<CreateCustomer, Customer>
{
    public async Task<Customer> Handle(CreateCustomer request, CancellationToken cancellationToken)
    {
        if (await customerRepository.ExistsAsync(request.Id, cancellationToken))
        {
            throw new CustomerAlreadyExistsException(request.Id);
        }

        var @event = new CustomerCreated(request.Id, request.Name, DateTime.UtcNow);
        var customer = Customer.Create(@event);

        await customerRepository.CreateAsync(customer.Id, @event, cancellationToken);
        return customer;
    }
}