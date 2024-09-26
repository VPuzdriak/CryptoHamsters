using CryptoHamsters.Wallets.Views;

using Marten;

using MediatR;

namespace CryptoHamsters.Wallets.Wallets.Transactions;

public sealed record GetWalletTransactions(Guid WalletId) : IRequest<WalletTransactions?>;

internal sealed class GetWalletTransactionsHandler(IQuerySession querySession)
    : IRequestHandler<GetWalletTransactions, WalletTransactions?>
{
    public Task<WalletTransactions?> Handle(
        GetWalletTransactions request,
        CancellationToken cancellationToken) =>
        querySession.Query<WalletTransactions>()
            .Where(t => t.Id == request.WalletId)
            .FirstOrDefaultAsync(cancellationToken);
}