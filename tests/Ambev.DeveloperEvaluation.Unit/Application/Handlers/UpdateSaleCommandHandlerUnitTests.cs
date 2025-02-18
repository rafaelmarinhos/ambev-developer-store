using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Handlers;

/// <summary>
/// Contains unit tests for the <see cref="UpdateSaleCommandHandler"/> class.
/// </summary>
public class UpdateSaleCommandHandlerUnitTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly UpdateSaleCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSaleCommandHandlerUnitTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public UpdateSaleCommandHandlerUnitTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new UpdateSaleCommandHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that handle fails when receive a invalid Id
    /// </summary>
    [Fact(DisplayName = "Given an invalid Id, When handling request, Then should throw Result.Fail")]
    public async Task Given_An_Invalid_Id_When_Handling_Request_Then_Should_Return_Result_Fail()
    {
        // Given
        var command = new UpdateSaleCommand
        {
            Id = Guid.Empty,
            Items = [new() { ProductId = Guid.NewGuid(), Quantity = 1, Price = 10.0m }]
        };

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().BeEquivalentTo("The field Id is required.");
    }

    /// <summary>
    /// Tests that handle fails when sale is not found by Id
    /// </summary>
    [Fact(DisplayName = "Given an non-existent Id, When handling request, Then should throw Result.Fail")]
    public async Task Given_An_Non_Existent_Id_When_Handling_Request_Then_Should_Return_Result_Fail()
    {
        // Given
        var id = Guid.NewGuid();
        var command = new UpdateSaleCommand
        {
            Id = id,
            Items = [new() { ProductId = Guid.NewGuid(), Quantity = 1, Price = 10.0m }]
        };

        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult<Sale?>(null));

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().BeEquivalentTo($"Sale with ID {id} not found.");
    }

    /// <summary>
    /// Tests that handle add a new product when productId not exists in the sale
    /// </summary>
    [Fact(DisplayName = "Given a valid command with a new product, When handling request, Then should add the product and return Result.Ok")]
    public async Task Given_A_Valid_Command_When_Handling_Request_Then_Should_Add_The_Product_And_Return_Result_Ok()
    {
        // Given
        var id = Guid.NewGuid();
        var command = new UpdateSaleCommand
        {
            Id = id,
            Items = [new() { ProductId = Guid.NewGuid(), Quantity = 5, Price = 10 }]
        };

        var existingSale = SaleHandlerTestData.GenerateSaleWithItems();        
        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(existingSale);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsFailed.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
    }
}
