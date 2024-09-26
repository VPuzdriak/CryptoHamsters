namespace CryptoHamsters.Service.GraphQL.Wallets.Customer;

public record CustomerWalletsPayload(
    Guid CustomerId,
    string CustomerName,
    IReadOnlyList<CustomerWalletSpecsPayload> Wallets);