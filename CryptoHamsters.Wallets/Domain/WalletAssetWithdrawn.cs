namespace CryptoHamsters.Wallets.Domain;

public record WalletAssetWithdrawn(Guid TransactionId, Guid WalletId, WalletAsset Asset, DateTime TimeStampUtc);