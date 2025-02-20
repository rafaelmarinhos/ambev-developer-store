using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.GetSale;

namespace Ambev.DeveloperEvaluation.Integration.Application.Sales;

/// <summary>
/// Contains integration tests for the <see cref="GetSaleHandler"/> class.
/// </summary>
public class GetSaleHandlerIntegrationTests : IClassFixture<IntegrationTestFixture>
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSaleHandlerIntegrationTests"/> class.
    /// Sets up the test with IntegrationTestFixture that contains all necessary services
    /// </summary>
    public GetSaleHandlerIntegrationTests(IntegrationTestFixture fixture)
    {
        _serviceProvider = fixture.ServiceProvider;
    }

    /// <summary>
    /// Tests that the command will be handled correcty by mediatR pipeline with a valid command and try to retry a different sale from DB
    /// </summary>
    [Fact(DisplayName = "Given an valid command, When handling request, Then should create a new sale on DB and return Result.Fail")]
    public async Task Handle_ValidCommand_ReturnsResultFailWhenSaleNotFound()
    {
        // 1 - Create a new sale

        // Given
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var createSaleCommand = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items =
            [
                new() { ProductId = Guid.NewGuid(), Quantity = 3, Price = 50.25m }
            ]
        };        

        // When
        var createSaleResult = await mediator.Send(createSaleCommand, CancellationToken.None);

        // Then
        createSaleResult.IsSuccess.Should().BeTrue();
        createSaleResult.IsFailed.Should().BeFalse();
        createSaleResult.Errors.Count.Should().Be(0);

        // 2 - Get the sale on DB by a different Id

        // Given
        var id = Guid.NewGuid();
        var getSaleCommand = new GetSaleCommand(id);

        // When
        var getSaleResult = await mediator.Send(getSaleCommand, CancellationToken.None);

        // Then
        getSaleResult.IsFailed.Should().BeTrue();
        getSaleResult.IsSuccess.Should().BeFalse();
        getSaleResult.Errors.Count.Should().Be(1);
        getSaleResult.Errors[0].Message.Should().BeEquivalentTo($"Sale with ID {id} not found.");
    }

    /// <summary>
    /// Tests that the command will be handled correcty by mediatR pipeline with a valid command and return a sale from DB
    /// </summary>
    [Fact(DisplayName = "Given an valid command, When handling request, Then should create a new sale on DB and return Ok")]
    public async Task Handle_ValidCommand_ReturnsResultOk()
    {
        // 1 - Create a new sale

        // Given
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var createSaleCommand = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items =
            [
                new() { ProductId = Guid.NewGuid(), Quantity = 3, Price = 50.25m }
            ]
        };

        // When
        var createSaleResult = await mediator.Send(createSaleCommand, CancellationToken.None);

        // Then
        createSaleResult.IsSuccess.Should().BeTrue();
        createSaleResult.IsFailed.Should().BeFalse();
        createSaleResult.Errors.Count.Should().Be(0);

        // 2 - Get the sale on DB by the Id recently created

        // Given
        var getSaleCommand = new GetSaleCommand(createSaleResult.Value.Id);

        // When
        var getSaleResult = await mediator.Send(getSaleCommand, CancellationToken.None);

        // Then
        getSaleResult.IsSuccess.Should().BeTrue();
        getSaleResult.IsFailed.Should().BeFalse();
        getSaleResult.Errors.Count.Should().Be(0);        
        getSaleResult.Value.CustomerId.Should().Be(createSaleCommand.CustomerId);
        getSaleResult.Value.BranchId.Should().Be(createSaleCommand.BranchId);
        getSaleResult.Value.Items.Count().Should().Be(1);
    }
}
