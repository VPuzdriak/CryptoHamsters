namespace CryptoHamsters.Wallets.Domain;

public sealed class NotEnoughFundsException(WalletAsset WalletAsset, Guid WalletId) : Exception("Not enough funds");