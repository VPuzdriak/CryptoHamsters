namespace CryptoHamsters.Wallets.Domain;

public record WalletCreated(
    Guid Id,
    Guid CustomerId,
    WalletType Type,
    DateTime CreatedAtUtc,
    Guid CausationId);