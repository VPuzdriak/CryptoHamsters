namespace CryptoHamsters.Wallets.Domain;

public record WalletToppedUp(Guid TransactionId, Guid WalletId, WalletAsset Asset, DateTime TimeStampUtc);