using CryptoHamsters.Customers.Create;

using HotChocolate.Types;

using MediatR;

namespace CryptoHamsters.Service.GraphQL.Customers;

public record CreateCustomerInput(string Name);

[MutationType]
public static class CreateCustomerMutation
{
    public static async Task<CustomerPayload> CreateCustomerAsync(
        CreateCustomerInput input,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new CreateCustomer(Guid.NewGuid(), input.Name);
        var customer = await mediator.Send(command, cancellationToken);
        return new CustomerPayload(customer.Id, customer.Name);
    }
}