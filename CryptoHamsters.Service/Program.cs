using CryptoHamsters.CryptoPairs;
using CryptoHamsters.CryptoPairs.Domain;
using CryptoHamsters.CryptoPairs.Infrastructure;
using CryptoHamsters.Customers;
using CryptoHamsters.Customers.Domain;
using CryptoHamsters.Orders;
using CryptoHamsters.Orders.Domain;
using CryptoHamsters.Service.GraphQL;
using CryptoHamsters.Service.Marten;
using CryptoHamsters.Service.MediatR;
using CryptoHamsters.Wallets;
using CryptoHamsters.Wallets.Domain;
using CryptoHamsters.Wallets.Infrastructure;
using CryptoHamsters.Wallets.Views;

using Marten;
using Marten.Events.Daemon.Coordination;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;

using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCryptoPairs();
builder.Services.AddCustomers();
builder.Services.AddWallets();
builder.Services.AddOrders();

builder.Services.ConfigureMediatR();
builder.Services.ConfigureMarten(builder.Configuration);
builder.Services.ConfigureGraphQL();

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