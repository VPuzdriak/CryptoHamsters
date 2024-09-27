using CryptoHamsters.CryptoPairs.Domain;
using CryptoHamsters.CryptoPairs.Infrastructure;
using CryptoHamsters.Customers.Domain;
using CryptoHamsters.Orders.Domain;
using CryptoHamsters.Wallets.Domain;
using CryptoHamsters.Wallets.Infrastructure;
using CryptoHamsters.Wallets.Views;

using Marten;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;

namespace CryptoHamsters.Service.Marten;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureMarten(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMarten(configure =>
            {
                configure.Connection(configuration.GetConnectionString("CryptoHamsters")!);
                configure.Events.DatabaseSchemaName = "events";

                configure.Schema.For<CryptoPair>().DatabaseSchemaName("crypto_pairs");
                configure.Projections.Snapshot<CryptoPair>(SnapshotLifecycle.Inline,
                    asyncConfig => asyncConfig.ProjectionName = "crypto_pairs");

                configure.Schema.For<Customer>().DatabaseSchemaName("customers");
                configure.Projections.Snapshot<Customer>(SnapshotLifecycle.Async,
                    asyncConfig => asyncConfig.ProjectionName = "customers");

                configure.Schema.For<Wallet>().DatabaseSchemaName("wallets");
                configure.Projections.Snapshot<Wallet>(SnapshotLifecycle.Async,
                    asyncConfig => asyncConfig.ProjectionName = "wallets");

                configure.Schema.For<CustomerWalletsProjection>().DatabaseSchemaName("wallets");
                configure.Projections.Add(
                    new CustomerWalletsProjection(),
                    ProjectionLifecycle.Async,
                    "customer_wallets");

                configure.Schema.For<WalletTransactions>().DatabaseSchemaName("wallets");
                configure.Projections.Add<WalletTransactionsProjection>(ProjectionLifecycle.Async);

                configure.Schema.For<MarketOrder>().DatabaseSchemaName("orders");
                configure.Projections.Snapshot<MarketOrder>(SnapshotLifecycle.Async,
                    asyncConfig => asyncConfig.ProjectionName = "market_orders");
            })
            .AddAsyncDaemon(DaemonMode.Solo)
            .AddSubscriptionWithServices<PriceChangedSubscription>(ServiceLifetime.Singleton, configure =>
            {
                configure.IncludeType<CryptoPairPriceChanged>();
            })
            .AddSubscriptionWithServices<CustomerCreatedSubscription>(ServiceLifetime.Scoped, configure =>
            {
                configure.IncludeType<CustomerCreated>();
            });

        return services;
    }
}