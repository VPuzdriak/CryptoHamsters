using CryptoHamsters.Wallets.TopUp;

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
            walletTransaction.AssetName,
            walletTransaction.Amount,
            walletTransaction.CreatedAtUtc);
    }
}