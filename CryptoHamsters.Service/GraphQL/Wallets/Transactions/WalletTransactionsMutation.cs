using CryptoHamsters.Wallets.Wallets;
using CryptoHamsters.Wallets.Wallets.TopUp;
using CryptoHamsters.Wallets.Wallets.Withdraw;

using MediatR;

namespace CryptoHamsters.Service.GraphQL.Wallets.Transactions;

[MutationType]
public static class WalletTransactionsMutation
{
    public static async Task<WalletTransactionPayload> TopUpWallet(
        TopUpWalletInput input,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var walletTransaction = await mediator.Send(
            new TopUpWallet(input.WalletId, input.AssetName, input.Amount),
            cancellationToken);

        return new WalletTransactionPayload(
            walletTransaction.Id,
            walletTransaction.WalletId,
            WalletTransactionType.TopUp,
            walletTransaction.AssetName,
            walletTransaction.Amount,
            walletTransaction.CreatedAtUtc);
    }

    public static async Task<WalletTransactionPayload> WithdrawAssetFromWallet(
        WithdrawAssetFromWalletInput input,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var walletTransaction = await mediator.Send(
            new WithdrawAssetFromWallet(input.WalletId, input.AssetName, input.Amount),
            cancellationToken);

        return new WalletTransactionPayload(
            walletTransaction.Id,
            walletTransaction.WalletId,
            WalletTransactionType.Withdraw,
            walletTransaction.AssetName,
            walletTransaction.Amount,
            walletTransaction.CreatedAtUtc);
    }
}