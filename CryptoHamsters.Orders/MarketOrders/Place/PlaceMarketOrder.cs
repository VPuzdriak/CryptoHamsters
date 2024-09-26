using CryptoHamsters.CryptoPairs.Domain;
using CryptoHamsters.CryptoPairs.Infrastructure;
using CryptoHamsters.Orders.Domain;
using CryptoHamsters.Orders.Infrastructure;
using CryptoHamsters.Wallets.Domain;
using CryptoHamsters.Wallets.Infrastructure;

using MediatR;

namespace CryptoHamsters.Orders.MarketOrders.Place;

public record PlaceMarketOrder(Guid WalletId, Guid CryptoPairId, decimal QuoteAssetAmount) : IRequest<MarketOrder>;

internal sealed class PlaceMarketOrderHandler(
    IMarketOrderRepository marketOrderRepository,
    IWalletRepository walletRepository,
    ICryptoPairsRepository cryptoPairsRepository)
    : IRequestHandler<PlaceMarketOrder, MarketOrder>
{
    public async Task<MarketOrder> Handle(PlaceMarketOrder request, CancellationToken cancellationToken)
    {
        var wallet = await walletRepository.GetAsync(request.WalletId, cancellationToken);
        if (wallet is null)
        {
            throw new WalletNotFoundException(request.WalletId);
        }

        var cryptoPair = await cryptoPairsRepository.GetAsync(request.CryptoPairId, cancellationToken);
        if (cryptoPair is null)
        {
            throw new CryptoPairNotFoundException(request.CryptoPairId);
        }

        var walletAsset = new WalletAsset(cryptoPair.QuoteAsset, request.QuoteAssetAmount);
        if (!wallet.CanWithdrawAsset(walletAsset))
        {
            throw new NotEnoughFundsException(walletAsset, request.WalletId);
        }

        var marketOrderPlaced = new MarketOrderPlaced(
            Guid.NewGuid(),
            request.WalletId,
            request.CryptoPairId,
            request.QuoteAssetAmount,
            DateTime.UtcNow);

        var marketOrder = MarketOrder.Create(marketOrderPlaced);

        await marketOrderRepository.CreateAsync(marketOrderPlaced.Id, marketOrderPlaced, cancellationToken);

        return marketOrder;
    }
}