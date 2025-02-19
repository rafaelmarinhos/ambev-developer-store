using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Integration.TestData;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Application.Sales;

/// <summary>
/// Contains integration tests for the <see cref="UpdateSaleCommandHandler"/> class.
/// </summary>
public class UpdateSaleHandlerIntegrationTests : IClassFixture<IntegrationTestFixture>
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSaleHandlerIntegrationTests"/> class.
    /// Sets up the test with IntegrationTestFixture that contains all necessary services
    /// </summary>
    public UpdateSaleHandlerIntegrationTests(IntegrationTestFixture fixture)
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

        var command = new UpdateSaleCommand
        {
            Id = Guid.Empty,
            Items = []
        };

        // When
        var result = await mediator.Send(command, CancellationToken.None);

        // Then
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors.Count.Should().Be(3);
        result.Errors[0].Message.Should().BeEquivalentTo("The field Id is required.");
        result.Errors[1].Message.Should().BeEquivalentTo("The sale must have at least one item.");
        result.Errors[2].Message.Should().BeEquivalentTo("The sale must have at least one valid item.");
    }

    /// <summary>
    /// Tests that the command will be handled correcty by mediatR pipeline with a invalid command
    /// </summary>
    [Fact(DisplayName = "Given an command invalid items, When handling request, Then should return Result.Fail with Errors")]
    public async Task Handle_InvalidCommandWithNoItems_ReturnsResultFail()
    {
        // Given
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var command = new UpdateSaleCommand
        {
            Id = Guid.NewGuid(),
            Items =
            [
                new() { ProductId = Guid.Empty, Quantity = 0, Price = 0 }
            ]
        };

        // When
        var result = await mediator.Send(command, CancellationToken.None);

        // Then
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors.Count.Should().Be(3);
        result.Errors[0].Message.Should().BeEquivalentTo("The field Product is required.");
        result.Errors[1].Message.Should().BeEquivalentTo("Quantity of this item must be greater than zero.");
        result.Errors[2].Message.Should().BeEquivalentTo("Price of this item must be greater than zero.");
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
        var command = new UpdateSaleCommand
        {
            Id = id,
            Items =
            [
                new() { ProductId = Guid.NewGuid(), Quantity = 10, Price = 100 }
            ]
        };

        // When
        var updatedSaleResult = await mediator.Send(command, CancellationToken.None);

        // Then
        updatedSaleResult.IsFailed.Should().BeTrue();
        updatedSaleResult.IsSuccess.Should().BeFalse();
        updatedSaleResult.Errors.Count.Should().Be(1);
        updatedSaleResult.Errors[0].Message.Should().BeEquivalentTo($"Sale with ID {id} not found.");
    }

    /// <summary>
    /// Tests that the command will be handled correcty by mediatR pipeline and add a new item
    /// </summary>
    [Fact(DisplayName = "Given an valid command, When handling request, Then should create a new sale on DB and after add a new item")]
    public async Task Handle_ValidCommand_ReturnsResultOkWithNewItem()
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
        createSaleResult.Value.TotalItems.Should().Be(1);

        // Update sale
        var updateSaleCommand = SaleHandlerTestData.GenerateUpdateSaleCommand(createSaleResult.Value.Id);

        var updateSaleProduct_01 = new UpdateSaleItemDto()
        {
            ProductId = SaleHandlerTestData.Product001,
            Quantity = 1,
            Price = 10
        };

        var updateSaleProduct_02 = new UpdateSaleItemDto()
        {
            ProductId = SaleHandlerTestData.Product002,
            Quantity = 2,
            Price = 20
        };

        updateSaleCommand.Items.Add(updateSaleProduct_01);
        updateSaleCommand.Items.Add(updateSaleProduct_02);

        var updatedSaleResult = await mediator.Send(updateSaleCommand, CancellationToken.None);

        // Then
        updatedSaleResult.IsFailed.Should().BeFalse();
        updatedSaleResult.IsSuccess.Should().BeTrue();
        updatedSaleResult.Value.TotalItems.Should().Be(2);
    }

    /// <summary>
    /// Tests that the command will be handled correcty by mediatR pipeline and cancel a item
    /// </summary>
    [Fact(DisplayName = "Given an valid command, When handling request, Then should create a new sale on DB and after cancel a item")]
    public async Task Handle_ValidCommand_ReturnsResultOkWithItemCanceled()
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
        createSaleResult.Value.TotalItems.Should().Be(1);

        // Update sale
        var updateSaleCommand = SaleHandlerTestData.GenerateUpdateSaleCommand(createSaleResult.Value.Id);

        var updateSaleProduct_01 = new UpdateSaleItemDto()
        {
            ProductId = SaleHandlerTestData.Product001,
            Quantity = 1,
            Price = 10,
            IsCanceled = true
        };

        updateSaleCommand.Items.Add(updateSaleProduct_01);

        var updatedSaleResult = await mediator.Send(updateSaleCommand, CancellationToken.None);

        // Then
        updatedSaleResult.IsFailed.Should().BeFalse();
        updatedSaleResult.IsSuccess.Should().BeTrue();
        updatedSaleResult.Value.TotalItems.Should().Be(1);
        updatedSaleResult.Value.Items.FirstOrDefault()!.IsCanceled.Should().BeTrue();
    }

    /// <summary>
    /// Tests that the command will be handled correcty by mediatR pipeline and cancel a item
    /// </summary>
    [Fact(DisplayName = "Given an valid command, When handling request, Then should create a new sale on DB and after update a item")]
    public async Task Handle_ValidCommand_ReturnsResultOkWithUpdatedItem()
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
        createSaleResult.Value.TotalItems.Should().Be(1);

        // Update sale
        var updateSaleCommand = SaleHandlerTestData.GenerateUpdateSaleCommand(createSaleResult.Value.Id);

        var updateSaleProduct_01 = new UpdateSaleItemDto()
        {
            ProductId = SaleHandlerTestData.Product001,
            Quantity = 4,
            Price = 25
        };

        updateSaleCommand.Items.Add(updateSaleProduct_01);

        var updatedSaleResult = await mediator.Send(updateSaleCommand, CancellationToken.None);

        // Then
        updatedSaleResult.IsFailed.Should().BeFalse();
        updatedSaleResult.IsSuccess.Should().BeTrue();
        updatedSaleResult.Value.TotalItems.Should().Be(1);
        updatedSaleResult.Value.Items.FirstOrDefault(f => f.ProductId == SaleHandlerTestData.Product001)!.Quantity.Should().Be(4);
        updatedSaleResult.Value.Items.FirstOrDefault(f => f.ProductId == SaleHandlerTestData.Product001)!.Price.Should().Be(25);
    }
}
