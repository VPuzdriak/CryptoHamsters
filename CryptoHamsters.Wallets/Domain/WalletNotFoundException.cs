namespace CryptoHamsters.Wallets.Domain;

public sealed class WalletNotFoundException(Guid WalletId) : Exception("Wallet not found");