namespace CryptoHamsters.Service.GraphQL;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureGraphQL(this IServiceCollection services)
    {
        services.AddGraphQLServer()
            .AddServiceTypes()
            .AddInMemorySubscriptions();

        return services;
    }
}