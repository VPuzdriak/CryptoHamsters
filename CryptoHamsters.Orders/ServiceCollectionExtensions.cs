using CryptoHamsters.Orders.Infrastructure;

using Microsoft.Extensions.DependencyInjection;

namespace CryptoHamsters.Orders;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrders(this IServiceCollection services)
    {
        services.AddScoped<IMarketOrderRepository, MarketOrderRepository>();
        return services;
    }
}