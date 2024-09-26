using CryptoHamsters.CryptoPairs.Domain;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CryptoHamsters.CryptoPairs.Infrastructure;

internal sealed class PriceChangerService(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var cryptoPairsRepository = scope.ServiceProvider.GetRequiredService<ICryptoPairsRepository>();

        while (!stoppingToken.IsCancellationRequested)
        {
            var cryptoPairs = await cryptoPairsRepository.GetCryptoPairsAsync(StableCoins.USDT, stoppingToken);

            foreach (var cryptoPair in cryptoPairs)
            {
                var priceChange = GetPriceChange(cryptoPair.Price);
                var priceChanged = new CryptoPairPriceChanged(cryptoPair.Id, priceChange, DateTime.UtcNow);

                await cryptoPairsRepository.UpdateWithAsync(cryptoPair.Id, priceChanged, stoppingToken);
            }

            await Task.Delay(2_000, stoppingToken);
        }
    }

    private decimal GetPriceChange(decimal lastPrice)
    {
        var multiplier = Random.Shared.Next(1, 3);
        var direction = Random.Shared.Next(0, 2) == 0 ? -1 : 1;

        return (lastPrice / 10 * multiplier * direction);
    }
}