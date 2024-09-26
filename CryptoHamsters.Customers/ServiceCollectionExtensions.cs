using CryptoHamsters.Customers.Infrastructure;

using Microsoft.Extensions.DependencyInjection;

namespace CryptoHamsters.Customers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomers(this IServiceCollection services)
    {
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        return services;
    }
}