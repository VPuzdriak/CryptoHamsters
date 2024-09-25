using CryptoHamsters.CryptoPairs.Domain;
using CryptoHamsters.CryptoPairs.Listing;

using Marten;

namespace CryptoHamsters.CryptoPairs.Infrastructure;

public interface ICryptoPairsRepository
{
    Task<bool> PairExistsAsync(Guid id, CancellationToken cancellationToken);

    Task CreateAsync(
        Guid pairId,
        CryptoPairListed cryptoPairListed,
        CancellationToken cancellationToken);
}

internal sealed class CryptoPairsRepository(IDocumentSession documentSession) : ICryptoPairsRepository
{
    public async Task<bool> PairExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        var cryptoPair = await documentSession.Events.AggregateStreamAsync<CryptoPair>(id, token: cancellationToken);
        return cryptoPair != null;
    }

    public async Task CreateAsync(
        Guid pairId,
        CryptoPairListed cryptoPairListed,
        CancellationToken cancellationToken)
    {
        documentSession.Events.StartStream<CryptoPair>(pairId, cryptoPairListed);
        await documentSession.SaveChangesAsync(cancellationToken);
    }
}