namespace CryptoHamsters.CryptoPairs.Domain;

public class CryptoPairAlreadyExistsException(Guid CryptoPairId) : Exception("Crypto pair already exists");