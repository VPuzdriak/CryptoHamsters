using CryptoHamsters.Wallets.Domain;
using CryptoHamsters.Wallets.Infrastructure;

using MediatR;

namespace CryptoHamsters.Wallets.Wallets.Withdraw;

public record WithdrawAssetFromWallet(Guid WalletId, string AssetName, decimal Amount) : IRequest<WalletTransaction>;

internal sealed class WithdrawAssetFromWalletHandler(IWalletRepository walletRepository)
    : IRequestHandler<WithdrawAssetFromWallet, WalletTransaction>
{
    public async Task<WalletTransaction> Handle(WithdrawAssetFromWallet request, CancellationToken cancellationToken)
    {
        Wallet? wallet = await walletRepository.GetAsync(request.WalletId, cancellationToken);

        if (wallet is null)
        {
            throw new WalletNotFoundException(request.WalletId);
        }

        var withdrawAsset = new WalletAsset(request.AssetName, request.Amount);

        if (!wallet.CanWithdrawAsset(withdrawAsset))
        {
            throw new NotEnoughFundsException(withdrawAsset, wallet.Id);
        }

        var assetWithdrawn = new WalletAssetWithdrawn(
            Guid.NewGuid(),
            request.WalletId,
            withdrawAsset,
            DateTime.UtcNow);

        await walletRepository.UpdateWithAsync(request.WalletId, assetWithdrawn, cancellationToken);

        return new WalletTransaction(
            assetWithdrawn.TransactionId,
            assetWithdrawn.WalletId,
            WalletTransactionType.Withdraw,
            assetWithdrawn.Asset.Name,
            assetWithdrawn.Asset.Amount,
            assetWithdrawn.TimeStampUtc);
    }
}