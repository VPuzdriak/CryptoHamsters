using CryptoHamsters.CryptoPairs.Domain;
using CryptoHamsters.CryptoPairs.Infrastructure;

using MediatR;

namespace CryptoHamsters.CryptoPairs.Listing;

public record ListCryptoPair(Guid Id, string Ticker, string BaseAsset, string QuoteAsset, decimal Price)
    : IRequest<CryptoPair>;

internal sealed class ListCryptoPairHandler(ICryptoPairsRepository cryptoPairsRepository)
    : IRequestHandler<ListCryptoPair, CryptoPair>
{
    public async Task<CryptoPair> Handle(ListCryptoPair request, CancellationToken cancellationToken)
    {
        if (await cryptoPairsRepository.PairExistsAsync(request.Id, cancellationToken))
        {
            throw new CryptoPairAlreadyExistsException(request.Id);
        }

        var @event = new CryptoPairListed(
            request.Id,
            request.Ticker,
            request.BaseAsset,
            request.QuoteAsset,
            DateTime.UtcNow,
            request.Price);

        var pair = CryptoPair.Create(@event);

        await cryptoPairsRepository.CreateAsync(pair.Id, @event, cancellationToken);

        return pair;
    }
}