namespace CryptoHamsters.Wallets.Domain;

public record WalletAssetWithdrawn(Guid WalletId, WalletAsset Asset, DateTime TimeStampUtc);