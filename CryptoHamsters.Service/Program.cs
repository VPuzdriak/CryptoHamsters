using CryptoHamsters.CryptoPairs;

using Marten;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCryptoPairs();

builder.Services.AddMediatR(configuration =>
    configuration.RegisterServicesFromAssemblies(
        AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName?.Contains("CryptoHamsters") is true)
            .ToArray()));

builder.Services.AddMarten(configure =>
{
    configure.Connection(builder.Configuration.GetConnectionString("CryptoHamsters")!);
    configure.Events.DatabaseSchemaName = "events";
});

builder.Services
    .AddGraphQLServer()
    .AddServiceTypes();

var app = builder.Build();

app.MapGraphQL();

app.Run();