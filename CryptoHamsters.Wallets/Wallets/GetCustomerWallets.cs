using CryptoHamsters.Wallets.Views;

using Marten;

using MediatR;

namespace CryptoHamsters.Wallets.Wallets;

public record GetCustomerWallets(Guid CustomerId) : IRequest<CustomerWalletsProjection?>;

internal sealed class GetCustomerWalletsHandler(IQuerySession session)
    : IRequestHandler<GetCustomerWallets, CustomerWalletsProjection?>
{
    public Task<CustomerWalletsProjection?> Handle(
        GetCustomerWallets request,
        CancellationToken cancellationToken) =>
        session.Query<CustomerWalletsProjection>()
            .Where(cw => cw.Id == request.CustomerId)
            .FirstOrDefaultAsync(cancellationToken);
}