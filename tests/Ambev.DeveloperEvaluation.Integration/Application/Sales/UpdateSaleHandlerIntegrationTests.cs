using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
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
    public async Task Given_An_Invalid_Command_When_Handling_Request_Then_Should_Return_Result_Fail_With_Errors()
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
    public async Task Given_An_Command_With_Invalid_Items_When_Handling_Request_Then_Should_Return_Result_Fail_With_Errors()
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
    public async Task Given_An_Valid_Command_When_Handling_Request_Then_Should_Return_Result_Fail_For_Not_Existent_Sale_By_Id()
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
    /// Tests that the command will be handled correcty by mediatR pipeline with a valid command and try to retry a different sale from DB to update
    /// </summary>
    //[Fact(DisplayName = "Given an valid command, When handling request, Then should create a new sale on DB and return Result.Fail for a not existent id on DB")]
    //public async Task Given_An_Valid_Command_When_Handling_Request_Then_Should_Create_Sale_On_DB_And_Return_Result_Fail_For_Not_Existent_Sale_By_Id()
    //{
    //    // 1 - Create a new sale

    //    // Given
    //    using var scope = _serviceProvider.CreateScope();
    //    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

    //    var createSaleCommand = new CreateSaleCommand
    //    {
    //        CustomerId = Guid.NewGuid(),
    //        BranchId = Guid.NewGuid(),
    //        Items =
    //        [
    //            new() { ProductId = Guid.NewGuid(), Quantity = 3, Price = 50.25m }
    //        ]
    //    };

    //    // When
    //    var createSaleResult = await mediator.Send(createSaleCommand, CancellationToken.None);

    //    // Then
    //    createSaleResult.IsSuccess.Should().BeTrue();
    //    createSaleResult.IsFailed.Should().BeFalse();
    //    createSaleResult.Errors.Count.Should().Be(0);

    //    // 2 - Get the sale on DB by a different Id

    //    // Given
    //    var id = Guid.NewGuid();
    //    var command = new UpdateSaleCommand
    //    {
    //        Id = id,
    //        Items =
    //        [
    //            new() { ProductId = Guid.Empty, Quantity = 0, Price = 0 }
    //        ]
    //    };

    //    // When
    //    var getSaleResult = await mediator.Send(command, CancellationToken.None);

    //    // Then
    //    getSaleResult.IsFailed.Should().BeTrue();
    //    getSaleResult.IsSuccess.Should().BeFalse();
    //    getSaleResult.Errors.Count.Should().Be(1);
    //    getSaleResult.Errors[0].Message.Should().BeEquivalentTo($"Sale with ID {id} not found.");
    //}
}
