namespace CryptoHamsters.Wallets.TopUp;

public record WalletTransaction(Guid Id, Guid WalletId, string AssetName, decimal Amount, DateTime CreatedAtUtc);