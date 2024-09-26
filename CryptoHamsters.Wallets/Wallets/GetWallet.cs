using CryptoHamsters.Wallets.Domain;
using CryptoHamsters.Wallets.Infrastructure;

using MediatR;

namespace CryptoHamsters.Wallets.Wallets;

public record GetWallet(Guid WalletId) : IRequest<Wallet?>;

internal sealed class GetWalletHandler(IWalletRepository walletRepository) : IRequestHandler<GetWallet, Wallet?>
{
    public Task<Wallet?> Handle(GetWallet request, CancellationToken cancellationToken) =>
        walletRepository.GetAsync(request.WalletId, cancellationToken);
}