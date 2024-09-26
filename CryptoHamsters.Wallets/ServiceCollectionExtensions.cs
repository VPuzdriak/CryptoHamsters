using CryptoHamsters.Wallets.Infrastructure;

using Microsoft.Extensions.DependencyInjection;

namespace CryptoHamsters.Wallets;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWallets(this IServiceCollection services)
    {
        services.AddScoped<IWalletRepository, WalletRepository>();
        return services;
    }
}