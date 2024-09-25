using CryptoHamsters.CryptoPairs.Domain;
using CryptoHamsters.CryptoPairs.Infrastructure;

using MediatR;

namespace CryptoHamsters.CryptoPairs.Listing;

public record GetCryptoPairs(string QuoteAsset) : IRequest<IReadOnlyList<CryptoPair>>;

internal sealed class GetCryptoPairsHandler(ICryptoPairsRepository cryptoPairsRepository)
    : IRequestHandler<GetCryptoPairs, IReadOnlyList<CryptoPair>>
{
    public Task<IReadOnlyList<CryptoPair>> Handle(GetCryptoPairs request, CancellationToken cancellationToken) => 
        cryptoPairsRepository.GetCryptoPairsAsync(request.QuoteAsset, cancellationToken);
}