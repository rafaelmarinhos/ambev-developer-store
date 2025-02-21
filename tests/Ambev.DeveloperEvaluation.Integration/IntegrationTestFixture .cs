using Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Testcontainers.PostgreSql;
using Xunit;
using ILoggerFactory = Castle.Core.Logging.ILoggerFactory;

namespace Ambev.DeveloperEvaluation.Integration;

/// <summary>
/// Contains necessary configuration for integration tests for the <see cref="CreateSaleHandler"/> class.
/// </summary>
public class IntegrationTestFixture : IAsyncLifetime
{
    public IServiceProvider ServiceProvider { get; private set; }

    // Use TestContainers for create a new postgress database on docker to run integration tests with real database operations
    private readonly PostgreSqlContainer _dbContainer;

    public IntegrationTestFixture()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("sales_db")
            .WithUsername("sales_username")
            .WithPassword("sales_password")
            .WithCleanUp(true)
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        var services = new ServiceCollection();

        // Add DBContext with PostgreSqlContainer created by TestContainers
        services.AddDbContext<DefaultContext>(options =>
            options.UseNpgsql(_dbContainer.GetConnectionString(), sql =>
                sql.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")));

        // Add MediatR and Handlers
        services.AddMediatR(f => f.RegisterServicesFromAssembly(typeof(CreateSaleCommandHandler).Assembly));

        // Add repositories
        services.AddScoped<ISaleRepository, SaleRepository>();

        // AutoMapper
        services.AddAutoMapper(typeof(CreateSaleCommandHandler).Assembly);

        // Adiciona logging
        var loggerMock = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
        services.AddSingleton(loggerMock);

        ServiceProvider = services.BuildServiceProvider();

        // Apply migrations before run tests
        using var scope = ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DefaultContext>();
        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}
