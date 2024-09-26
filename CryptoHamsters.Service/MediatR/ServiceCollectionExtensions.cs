namespace CryptoHamsters.Service.MediatR;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureMediatR(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssemblies(
                AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => a.FullName?.Contains("CryptoHamsters") is true)
                    .ToArray()));
        
        return services;
    }
}