namespace CryptoHamsters.CryptoPairs.Domain;

public sealed class CryptoPairNotFoundException(Guid Id) : Exception("Crypto pair not found");