using CryptoHamsters.Wallets.Domain;
using CryptoHamsters.Wallets.Wallets;

using MediatR;

namespace CryptoHamsters.Service.GraphQL.Wallets.Get;

[QueryType]
public static class WalletQueries
{
    public static async Task<WalletPayload?> GetWallet(Guid walletId, IMediator mediator)
    {
        var wallet = await mediator.Send(new GetWallet(walletId));

        if (wallet is null)
        {
            return null;
        }

        var assets = wallet.Assets.Select(a => new WalletAssetPayload(a.Name, a.Amount)).ToArray();
        return new WalletPayload(wallet.Id, wallet.Type, assets, wallet.CreatedAtUtc);
    }
}

public record WalletPayload(Guid Id, WalletType Type, WalletAssetPayload[] Assets, DateTime CreatedAtUtc);

public record WalletAssetPayload(string Name, decimal Amount);