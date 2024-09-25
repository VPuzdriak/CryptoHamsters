using CryptoHamsters.CryptoPairs.Listing;

using HotChocolate.Types;

using MediatR;

namespace CryptoHamsters.Service.GraphQL.CryptoPairs;

[QueryType]
public static class GetCryptoPairsQuery
{
    public static async Task<IReadOnlyList<CryptoPairPayload>> GetCryptoPairs(string quoteAsset, IMediator mediator,
        CancellationToken cancellationToken)
    {
        var cryptoPairs = await mediator.Send(new GetCryptoPairs(quoteAsset), cancellationToken);
        return cryptoPairs.Select(cp =>
                new CryptoPairPayload(
                    cp.Id,
                    cp.Ticker,
                    cp.ListedAtUtc,
                    cp.Price,
                    cp.PriceUpdatedAtUtc))
            .ToList();
    }
}