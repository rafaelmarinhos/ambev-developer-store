﻿using Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Integration.TestData;

namespace Ambev.DeveloperEvaluation.Integration.Application.Sales;

/// <summary>
/// Contains integration tests for the <see cref="CreateSaleHandler"/> class.
/// </summary>
public class CreateSaleHandlerIntegrationTests : IClassFixture<IntegrationTestFixture>
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleHandlerIntegrationTests"/> class.
    /// Sets up the test with IntegrationTestFixture that contains all necessary services
    /// </summary>
    public CreateSaleHandlerIntegrationTests(IntegrationTestFixture fixture)
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

        var createSaleCommand = new CreateSaleCommand
        {
            CustomerId = Guid.Empty,
            BranchId = Guid.Empty,
            Items =
            [
                new() { ProductId = Guid.NewGuid(), Quantity = 3, Price = 50.25m }
            ]
        };

        // When
        var result = await mediator.Send(createSaleCommand, CancellationToken.None);

        // Then
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors.Count.Should().Be(2);
        result.Errors[0].Message.Should().BeEquivalentTo("The field Customer is required.");
        result.Errors[1].Message.Should().BeEquivalentTo("The field Branch is required.");
    }

    /// <summary>
    /// Tests that the command will be handled correcty by mediatR pipeline with a valid command
    /// </summary>
    [Fact(DisplayName = "Given an valid command, When handling request, Then should create a new sale on DB")]
    public async Task Handle_ValidCommand_ReturnsResultOk()
    {
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
        var result = await mediator.Send(createSaleCommand, CancellationToken.None);

        // Then
        result.IsSuccess.Should().BeTrue();
        result.IsFailed.Should().BeFalse();
        result.Errors.Count.Should().Be(0);
    }

    /// <summary>
    /// Tests that the command will be handled correcty by mediatR pipeline with a valid command and calculate total amount e discounts
    /// </summary>
    [Fact(DisplayName = "Given an valid command, When handling request, Then should create a new sale on DB and calculate total amount and discounts")]
    public async Task Handle_ValidCommand_ReturnsResultOkWithTotalAmountsAndDiscounts()
    {
        // Given
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var createSaleCommand = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items =
            [
                new() { ProductId = SaleHandlerTestData.Product001, Quantity = 3, Price = 10 },
                new() { ProductId = SaleHandlerTestData.Product002, Quantity = 9, Price = 10 },
                new() { ProductId = SaleHandlerTestData.Product003, Quantity = 15, Price = 10 }
            ]
        };

        // When
        var result = await mediator.Send(createSaleCommand, CancellationToken.None);

        // Then
        result.IsSuccess.Should().BeTrue();
        result.IsFailed.Should().BeFalse();
        result.Errors.Count.Should().Be(0);
        result.Value.TotalAmount.Should().Be(270);
        result.Value.Discount.Should().Be(39);
    }
}