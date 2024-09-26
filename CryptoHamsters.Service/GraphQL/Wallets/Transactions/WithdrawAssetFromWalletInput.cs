namespace CryptoHamsters.Service.GraphQL.Wallets.Transactions;

public record WithdrawAssetFromWalletInput(
    Guid WalletId,
    string AssetName,
    decimal Amount);