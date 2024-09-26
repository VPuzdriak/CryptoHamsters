using CryptoHamsters.CryptoPairs.Infrastructure;

using Microsoft.Extensions.DependencyInjection;

namespace CryptoHamsters.CryptoPairs;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCryptoPairs(this IServiceCollection services)
    {
        services.AddScoped<ICryptoPairsRepository, CryptoPairsRepository>();
        services.AddHostedService<PriceChangerService>();
        
        return services;
    }
}