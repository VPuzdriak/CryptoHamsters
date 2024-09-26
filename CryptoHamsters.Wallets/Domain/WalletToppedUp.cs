namespace CryptoHamsters.Wallets.Domain;

public record WalletToppedUp(Guid WalletId, WalletAsset Asset, DateTime TimeStampUtc);