using CryptoHamsters.Wallets.Wallets.Transactions;

using MediatR;

namespace CryptoHamsters.Service.GraphQL.Wallets.Transactions;

[QueryType]
public static class GetWalletTransactionsQuery
{
    public static async Task<WalletTransactionsPayload?> GetWalletTransactions(
        Guid walletId,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var walletTransactions = await mediator.Send(new GetWalletTransactions(walletId), cancellationToken);

        if (walletTransactions is null)
        {
            return null;
        }

        return new WalletTransactionsPayload(
            walletTransactions.Id,
            walletTransactions.Transactions
                .Select(t => new WalletTransactionPayload(
                    t.Id,
                    t.WalletId,
                    t.TransactionType,
                    t.AssetName,
                    t.Amount,
                    t.CreatedAtUtc))
                .ToList());
    }
}

public record WalletTransactionsPayload(Guid WalletId, IReadOnlyList<WalletTransactionPayload> Transactions);