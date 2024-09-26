using CryptoHamsters.CryptoPairs.Listing;

using MediatR;

namespace CryptoHamsters.Service.GraphQL.CryptoPairs.Listing;

public record ListCryptoPairInput(string Ticker, string BaseAsset, string QuoteAsset, decimal Price);

[MutationType]
public static class ListCryptoPairMutation
{
    public static async Task<CryptoPairPayload> ListCryptoPair(
        ListCryptoPairInput input,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var cryptoPair = await mediator.Send(
            new ListCryptoPair(Guid.NewGuid(), input.Ticker, input.BaseAsset, input.QuoteAsset, input.Price),
            cancellationToken);

        return new CryptoPairPayload(
            cryptoPair.Id,
            cryptoPair.Ticker,
            cryptoPair.ListedAtUtc,
            cryptoPair.Price,
            cryptoPair.PriceUpdatedAtUtc);
    }
}