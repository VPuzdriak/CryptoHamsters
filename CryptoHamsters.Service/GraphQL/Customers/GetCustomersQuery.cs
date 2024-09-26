using CryptoHamsters.Customers.Create;

using HotChocolate.Types;

using MediatR;

namespace CryptoHamsters.Service.GraphQL.Customers;

[QueryType]
public static class GetCustomersQuery
{
    public static async Task<IReadOnlyList<CustomerPayload>> GetCustomersAsync(
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var customers = await mediator.Send(new GetCustomers(), cancellationToken);
        return customers.Select(c => new CustomerPayload(c.Id, c.Name)).ToList();
    }
}