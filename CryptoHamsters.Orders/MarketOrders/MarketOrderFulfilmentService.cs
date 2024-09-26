using CryptoHamsters.CryptoPairs.Infrastructure;
using CryptoHamsters.Orders.Domain;
using CryptoHamsters.Orders.Infrastructure;
using CryptoHamsters.Wallets.Domain;
using CryptoHamsters.Wallets.Infrastructure;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CryptoHamsters.Orders.MarketOrders;

internal sealed class MarketOrderFulfilmentService(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var marketOrderRepository = scope.ServiceProvider.GetRequiredService<IMarketOrderRepository>();
        var cryptoPairRepository = scope.ServiceProvider.GetRequiredService<ICryptoPairsRepository>();
        var walletRepository = scope.ServiceProvider.GetRequiredService<IWalletRepository>();

        while (!stoppingToken.IsCancellationRequested)
        {
            var placedOrders = await marketOrderRepository.GetPlacedOrdersAsync(stoppingToken, first: 20);

            foreach (var placedOrder in placedOrders)
            {
                var cryptoPair = await cryptoPairRepository.GetAsync(placedOrder.CryptoPairId, stoppingToken);

                if (cryptoPair == null)
                {
                    continue;
                }

                var fulfilledPrice = cryptoPair.Price;
                var fulfilledAmount = placedOrder.QuoteAssetAmount / fulfilledPrice;

                var walletAssetWithdrawn = new WalletAssetWithdrawn(
                    Guid.NewGuid(),
                    placedOrder.WalletId,
                    new WalletAsset(cryptoPair.QuoteAsset, placedOrder.QuoteAssetAmount),
                    DateTime.UtcNow);

                var walletToppedUp = new WalletToppedUp(
                    Guid.NewGuid(),
                    placedOrder.WalletId,
                    new WalletAsset(cryptoPair.BaseAsset, fulfilledAmount),
                    DateTime.UtcNow);

                await walletRepository.UpdateWithAsync(
                    placedOrder.WalletId,
                    stoppingToken,
                    walletAssetWithdrawn,
                    walletToppedUp);

                var orderFulfilled = new MarketOrderFulfilled(
                    placedOrder.Id,
                    fulfilledAmount,
                    fulfilledPrice,
                    DateTime.UtcNow);

                await marketOrderRepository.UpdateWithAsync(placedOrder.Id, orderFulfilled, CancellationToken.None);
            }
            
            await Task.Delay(4_000, stoppingToken);
        }
    }
}