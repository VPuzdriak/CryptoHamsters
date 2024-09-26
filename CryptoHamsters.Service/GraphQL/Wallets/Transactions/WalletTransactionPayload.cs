using CryptoHamsters.Wallets.Wallets;

namespace CryptoHamsters.Service.GraphQL.Wallets.Transactions;

public record WalletTransactionPayload(
    Guid Id,
    Guid WalletId,
    WalletTransactionType TransactionType,
    string AssetName,
    decimal Amount,
    DateTime CreatedAtUtc
);