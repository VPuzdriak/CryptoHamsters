using HotChocolate.Types;

namespace CryptoHamsters.Service.GraphQL.CryptoPairs;

[QueryType]
public static class GetCryptoPairsQuery
{
    public static IReadOnlyList<CryptoPairPayload> GetCryptoPairs() => [];
}