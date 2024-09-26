using CryptoHamsters.Wallets.Domain;

namespace CryptoHamsters.Service.GraphQL.Wallets.Customer;

public record CustomerWalletSpecsPayload(
    Guid WalletId,
    WalletType Type,
    DateTime CreatedAtUtc);