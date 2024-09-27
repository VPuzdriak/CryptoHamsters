using CryptoHamsters.Wallets.Wallets;

using MediatR;

namespace CryptoHamsters.Service.GraphQL.Wallets.Customer;

[QueryType]
public static class CustomerWalletsQuery
{
    public static async Task<CustomerWalletsPayload?> GetCustomerWallets(Guid customerId, IMediator mediator)
    {
        var customerWallets = await mediator.Send(new GetCustomerWallets(customerId));
        if (customerWallets is null)
        {
            return null;
        }

        return new CustomerWalletsPayload(
            customerWallets.Id,
            customerWallets.CustomerName,
            customerWallets.Wallets
                .Select(w => new CustomerWalletSpecsPayload(w.Id, w.Type, w.Assets, w.CreatedAtUtc))
                .ToList());
    }
}