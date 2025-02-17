using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Handlers;

/// <summary>
/// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
/// </summary>
public class CreateSaleHandlerUnitTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly CreateSaleCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleHandlerUnitTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CreateSaleHandlerUnitTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateSaleCommandHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that handle fails when receive a invalid command
    /// </summary>
    [Fact(DisplayName = "Given an invalid command, When handling request, Then should throw ValidationException")]
    public async Task Given_An_Invalid_Command_When_Handling_Request_Then_Should_Throw_ValidationException()
    {
        // Given
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.Empty, // Invalid customerId
            BranchId = Guid.NewGuid(),
            Items = [new() { ProductId = Guid.NewGuid(), Quantity = 1, Price = 10.0m }]
        };

        // When
        var result = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await result.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that the command is mapped to sale entity successfully
    /// </summary>
    [Fact(DisplayName = "Given a valid command, When handling request, Then should map command to Sale entity")]
    public async Task Given_A_Valid_Command_When_Handling_Request_Then_Should_Map_Command_To_Sale()
    {
        // Given
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items = [new() { ProductId = Guid.NewGuid(), Quantity = 1, Price = 10.0m }]
        };

        var sale = new Sale();
        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<Sale>(command);
    }

    /// <summary>
    /// Tests that CreateAsync in the sales repository is called correct
    /// </summary>
    [Fact(DisplayName = "Given a valid command, When handling request, Then should persist sale in repository")]
    public async Task Given_A_Valid_Command_When_Handling_Request_Then_Should_Persist_Sale_In_Repository()
    {
        // Given
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items = [new() { ProductId = Guid.NewGuid(), Quantity = 1, Price = 10.0m }]
        };

        var sale = new Sale();
        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(sale));

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).CreateAsync(sale, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the sale entity is mapped to result successfully
    /// </summary>
    [Fact(DisplayName = "Given a created sale, When handling request, Then should return mapped CreateSaleResult")]
    public async Task Given_A_Created_Sale_When_Handling_Request_Then_Should_Return_Mapped_CreateSaleResult()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items = [new() { ProductId = Guid.NewGuid(), Quantity = 1, Price = 10.0m }]
        };

        var sale = new Sale();
        var expectedResult = new CreateSaleResult();

        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(sale));
        _mapper.Map<CreateSaleResult>(sale).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResult, result);
        _mapper.Received(1).Map<CreateSaleResult>(sale);
    }

    /// <summary>
    /// Tests that the sale entity is mapped to result successfully with the correct sale number
    /// </summary>
    [Fact(DisplayName = "Given a created sale, When handling request, Then should return mapped CreateSaleResult with sale number")]
    public async Task Given_A_Created_Sale_When_Handling_Request_Then_Should_Return_Mapped_CreateSaleResult_With_Sale_Number()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items = [new() { ProductId = Guid.NewGuid(), Quantity = 1, Price = 10.0m }]
        };

        var sale = new Sale();
        var expectedResult = CreateSaleHandlerTestData.GenerateCreateSaleResult();

        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(sale));
        _mapper.Map<CreateSaleResult>(sale).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResult, result);
        Assert.Equal(101010, expectedResult.Number);
        _mapper.Received(1).Map<CreateSaleResult>(sale);
    }
}
