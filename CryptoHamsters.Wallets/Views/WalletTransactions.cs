using CryptoHamsters.Wallets.Domain;
using CryptoHamsters.Wallets.Wallets;

using Marten.Events.Aggregation;

namespace CryptoHamsters.Wallets.Views;

public sealed record WalletTransactions(Guid Id, List<WalletTransaction> Transactions);

public sealed class WalletTransactionsProjection : SingleStreamProjection<WalletTransactions>
{
    public WalletTransactionsProjection()
    {
        ProjectionName = "wallet_transactions";
    }

    public static WalletTransactions Create(WalletCreated walletCreated) =>
        new(walletCreated.Id, []);

    public WalletTransactions Apply(WalletTransactions walletTransactions, WalletToppedUp @event) =>
        walletTransactions with
        {
            Transactions =
            [
                ..walletTransactions.Transactions,
                new WalletTransaction(
                    @event.TransactionId,
                    @event.WalletId,
                    WalletTransactionType.TopUp,
                    @event.Asset.Name,
                    @event.Asset.Amount,
                    @event.TimeStampUtc)
            ]
        };

    public WalletTransactions Apply(WalletTransactions walletTransactions, WalletAssetWithdrawn @event) =>
        walletTransactions with
        {
            Transactions =
            [
                ..walletTransactions.Transactions,
                new WalletTransaction(
                    @event.TransactionId,
                    @event.WalletId,
                    WalletTransactionType.Withdraw,
                    @event.Asset.Name,
                    @event.Asset.Amount,
                    @event.TimeStampUtc)
            ]
        };
}