namespace CryptoHamsters.Wallets.Wallets;

public record WalletTransaction(
    Guid Id,
    Guid WalletId,
    WalletTransactionType TransactionType,
    string AssetName,
    decimal Amount,
    DateTime CreatedAtUtc);

public enum WalletTransactionType
{
    TopUp = 0,
    Withdraw = 1
}