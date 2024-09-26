namespace CryptoHamsters.Service.GraphQL.Wallets.Transactions;

public record WalletTransactionPayload(
    Guid Id,
    Guid WalletId,
    string AssetName,
    decimal Amount,
    DateTime CreatedAtUtc
);