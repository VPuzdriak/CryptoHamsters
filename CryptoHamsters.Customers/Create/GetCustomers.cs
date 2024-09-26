using CryptoHamsters.Customers.Domain;

using Marten;

using MediatR;

namespace CryptoHamsters.Customers.Create;

public record GetCustomers : IRequest<IReadOnlyList<Customer>>;

internal sealed class GetCustomersHandler(IDocumentSession documentSession)
    : IRequestHandler<GetCustomers, IReadOnlyList<Customer>>
{
    public Task<IReadOnlyList<Customer>> Handle(GetCustomers request, CancellationToken cancellationToken) =>
        documentSession.Query<Customer>().ToListAsync(cancellationToken);
}