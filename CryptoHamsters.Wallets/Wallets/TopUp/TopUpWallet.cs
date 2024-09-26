using CryptoHamsters.Wallets.Domain;
using CryptoHamsters.Wallets.Infrastructure;

using MediatR;

namespace CryptoHamsters.Wallets.Wallets.TopUp;

public record TopUpWallet(Guid WalletId, string AssetName, decimal Amount) : IRequest<WalletTransaction>;

internal sealed class TopUpWalletHandler(IWalletRepository walletRepository)
    : IRequestHandler<TopUpWallet, WalletTransaction>
{
    public async Task<WalletTransaction> Handle(TopUpWallet request, CancellationToken cancellationToken)
    {
        var toppedUp = new WalletToppedUp(
            Guid.NewGuid(),
            request.WalletId,
            new WalletAsset(request.AssetName, request.Amount),
            DateTime.UtcNow);

        await walletRepository.UpdateWithAsync(request.WalletId, toppedUp, cancellationToken);

        return new WalletTransaction(
            toppedUp.TransactionId,
            toppedUp.WalletId,
            WalletTransactionType.TopUp,
            toppedUp.Asset.Name,
            toppedUp.Asset.Amount,
            toppedUp.TimeStampUtc);
    }
}