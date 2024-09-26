using CryptoHamsters.Wallets.Views;

using Marten;

using MediatR;

namespace CryptoHamsters.Wallets.Wallets;

public record GetCustomerWallets(Guid CustomerId) : IRequest<CustomerWallets?>;

internal sealed class GetCustomerWalletsHandler(IQuerySession session)
    : IRequestHandler<GetCustomerWallets, CustomerWallets?>
{
    public Task<CustomerWallets?> Handle(
        GetCustomerWallets request,
        CancellationToken cancellationToken) =>
        session.Query<CustomerWallets>()
            .Where(cw => cw.Id == request.CustomerId)
            .FirstOrDefaultAsync(cancellationToken);
}