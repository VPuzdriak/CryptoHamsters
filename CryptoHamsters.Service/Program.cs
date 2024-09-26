using CryptoHamsters.CryptoPairs;
using CryptoHamsters.CryptoPairs.Domain;
using CryptoHamsters.CryptoPairs.Infrastructure;
using CryptoHamsters.Customers;
using CryptoHamsters.Customers.Domain;

using Marten;
using Marten.Events.Daemon.Coordination;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;

using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCryptoPairs();
builder.Services.AddCustomers();

builder.Services.AddMediatR(configuration =>
    configuration.RegisterServicesFromAssemblies(
        AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName?.Contains("CryptoHamsters") is true)
            .ToArray()));

builder.Services
    .AddMarten(configure =>
    {
        configure.Connection(builder.Configuration.GetConnectionString("CryptoHamsters")!);
        configure.Events.DatabaseSchemaName = "events";

        configure.Schema.For<CryptoPair>().DatabaseSchemaName("crypto_pairs");
        configure.Projections.Snapshot<CryptoPair>(SnapshotLifecycle.Inline,
            asyncConfig => asyncConfig.ProjectionName = "crypto_pairs");

        configure.Schema.For<Customer>().DatabaseSchemaName("customers");
        configure.Projections.Snapshot<Customer>(SnapshotLifecycle.Async,
            asyncConfig => asyncConfig.ProjectionName = "customers");
    })
    .AddAsyncDaemon(DaemonMode.Solo)
    .AddSubscriptionWithServices<PriceChangedSubscription>(ServiceLifetime.Singleton, configure =>
    {
        configure.IncludeType<CryptoPairPriceChanged>();
    });

builder.Services
    .AddGraphQLServer()
    .AddServiceTypes()
    .AddInMemorySubscriptions();

var app = builder.Build();

app.UseWebSockets();
app.MapGraphQL();

app.MapGet("/reset/{projectionName}",
    async (string projectionName, [FromServices] IProjectionCoordinator projectionCoordinator,
        CancellationToken cancellationToken) =>
    {
        var daemon = projectionCoordinator.DaemonForMainDatabase();
        await daemon.RebuildProjectionAsync(projectionName, cancellationToken);
        return Results.Ok($"Reset {projectionName}");
    });

app.Run();