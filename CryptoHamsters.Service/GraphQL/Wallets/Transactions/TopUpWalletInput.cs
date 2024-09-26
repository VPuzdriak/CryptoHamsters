namespace CryptoHamsters.Service.GraphQL.Wallets.Transactions;

public record TopUpWalletInput(
    Guid WalletId,
    string AssetName,
    decimal Amount
);