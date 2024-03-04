using System;
using System.IO;
using System.Reflection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RandomDataFetcher.Application;
using RandomDataFetcher.Domain;
using RandomDataFetcher.FunctionApp;
using RandomDataFetcher.Infrastructure;
using DependencyInjection = RandomDataFetcher.Application.DependencyInjection;
[assembly: FunctionsStartup(typeof(Startup))]
namespace RandomDataFetcher.FunctionApp;

public class Startup : FunctionsStartup
{
    private IConfiguration _configuration;

    public override void Configure(IFunctionsHostBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (_configuration == null)
        {
            throw new InvalidOperationException("Configuration was not initialized");
        }

        builder.Services
            .AddCoreServices()
            .AddInfrastructure(_configuration)
            .AddApplicationServices();

        builder.Services.AddMediatR(static config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            config.RegisterServicesFromAssembly(typeof(Domain.DependencyInjection).Assembly);
            config.RegisterServicesFromAssembly(typeof(Infrastructure.DependencyInjection).Assembly);
        });
    }

    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        var configPath = builder.GetContext().ApplicationRootPath;

        builder.ConfigurationBuilder
            .AddJsonFile(Path.Combine(configPath, "appsettings.env.json"), true)
            .AddJsonFile("appsettings.env.json", true)
            .AddJsonFile(Path.Combine(configPath, "appsettings.static.json"), true)
            .AddJsonFile("appsettings.static.json", true)
            .AddEnvironmentVariables()
            .AddUserSecrets(Assembly.GetAssembly(GetType())!, true);

        _configuration = builder.ConfigurationBuilder.Build();
    }
}
