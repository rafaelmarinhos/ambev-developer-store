using Ambev.DeveloperEvaluation.Application.Sales.Commands.UpdateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;
using Ambev.DeveloperEvaluation.Integration.TestData;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.GetSale;

namespace Ambev.DeveloperEvaluation.Integration.Application.Sales;

/// <summary>
/// Contains integration tests for the <see cref="DeleteSaleCommandHandler"/> class.
/// </summary>
public class DeleteSaleHandlerIntegrationTests : IClassFixture<IntegrationTestFixture>
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteSaleHandlerIntegrationTests"/> class.
    /// Sets up the test with IntegrationTestFixture that contains all necessary services
    /// </summary>
    public DeleteSaleHandlerIntegrationTests(IntegrationTestFixture fixture)
    {
        _serviceProvider = fixture.ServiceProvider;
    }

    /// <summary>
    /// Tests that the command will be handled correcty by mediatR pipeline with a invalid command
    /// </summary>
    [Fact(DisplayName = "Given an invalid command, When handling request, Then should return Result.Fail with Errors")]
    public async Task Handle_InvalidCommand_ReturnsResultFail()
    {
        // Given
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        
        var command = new DeleteSaleCommand(Guid.Empty);

        // When
        var result = await mediator.Send(command, CancellationToken.None);

        // Then
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors.Count.Should().Be(1);
        result.Errors[0].Message.Should().BeEquivalentTo("Sale ID is required.");
    }

    /// <summary>
    /// Tests that the command will be handled correcty by mediatR pipeline with a valid command and try to retry a different sale from DB to update
    /// </summary>
    [Fact(DisplayName = "Given an valid command, When handling request, Then should return Result.Fail for a not existent id on DB")]
    public async Task Handle_ValidCommand_ReturnsResultFailSaleNotFound()
    {
        // Given
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        // Given
        var id = Guid.NewGuid();
        var command = new DeleteSaleCommand(id);

        // When
        var result = await mediator.Send(command, CancellationToken.None);

        // Then
        result.IsFailed.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors[0].Message.Should().BeEquivalentTo($"Sale with ID {id} not found.");
    }

    /// <summary>
    /// Tests that the command will be handled correcty by mediatR pipeline and add a new item and cancel them
    /// </summary>
    [Fact(DisplayName = "Given an valid command, When handling request, Then should create a new sale on DB and after cancel them")]
    public async Task Handle_ValidCommand_ReturnsResultOkCancelSale()
    {
        // Given
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        // When

        // Create sale
        var createSaleCommand = SaleHandlerTestData.GenerateCreateSaleCommand();

        var createSaleProduct_01 = new CreateSaleItemDto()
        {
            ProductId = SaleHandlerTestData.Product001,
            Quantity = 1,
            Price = 10
        };

        createSaleCommand.Items.Add(createSaleProduct_01);

        var createSaleResult = await mediator.Send(createSaleCommand, CancellationToken.None);
        createSaleResult.IsFailed.Should().BeFalse();
        createSaleResult.IsSuccess.Should().BeTrue();

        // Get sale
        var getSaleCommand = new GetSaleCommand(createSaleResult.Value.Id);        
        var getSaleResult = await mediator.Send(getSaleCommand, CancellationToken.None);
        getSaleResult.Value.Cancelled.Should().BeFalse(); // Here the is not canceled

        // Delete sale
        var command = new DeleteSaleCommand(createSaleResult.Value.Id);
        var result = await mediator.Send(command, CancellationToken.None);

        // Then
        result.IsFailed.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value.Cancelled.Should().BeTrue();
    }
}
