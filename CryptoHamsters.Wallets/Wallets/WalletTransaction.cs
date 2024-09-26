namespace CryptoHamsters.Wallets.Wallets;

public record WalletTransaction(Guid Id, Guid WalletId, string AssetName, decimal Amount, DateTime CreatedAtUtc);