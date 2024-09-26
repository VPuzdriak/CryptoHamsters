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
        while (true)
        {
            // Price goes down or up
            var direction = Random.Shared.Next(0, 2) == 0 ? -1 : 1;

            decimal change = lastPrice switch
            {
                >= 10_000 => Random.Shared.Next(0, 101),
                >= 1_000 => Random.Shared.Next(0, 11),
                _ => Random.Shared.Next(0, 3)
            } * direction;

            var newPrice = lastPrice + change;

            // Price should be greater than 0. For demo purposes, we don't want to have negative prices
            if (newPrice > 0)
            {
                return change;
            }
        }
    }
}