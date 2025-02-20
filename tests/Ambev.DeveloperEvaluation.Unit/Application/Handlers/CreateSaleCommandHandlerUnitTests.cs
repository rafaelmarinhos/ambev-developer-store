using Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;
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
public class CreateSaleCommandHandlerUnitTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly CreateSaleCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleCommandHandlerUnitTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CreateSaleCommandHandlerUnitTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateSaleCommandHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that handle fails when receive a invalid customerId
    /// </summary>
    [Fact(DisplayName = "Given an invalid customerId, When handling request, Then should throw Result.Fail")]
    public async Task Handle_InvalidCommandWithInvalidCustomer_ReturnsResultFail()
    {
        // Given
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.Empty,
            BranchId = Guid.NewGuid(),
            Items = [new() { ProductId = Guid.NewGuid(), Quantity = 1, Price = 10.0m }]
        };

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().BeEquivalentTo("The field Customer is required.");
    }

    /// <summary>
    /// Tests that handle fails when receive a invalid branchId
    /// </summary>
    [Fact(DisplayName = "Given an invalid branchId, When handling request, Then should throw Result.Fail")]
    public async Task Handle_InvalidCommandWithInvalidBranch_ReturnsResultFail()
    {
        // Given
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.Empty,
            Items = [new() { ProductId = Guid.NewGuid(), Quantity = 1, Price = 10.0m }]
        };

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().BeEquivalentTo("The field Branch is required.");
    }

    /// <summary>
    /// Tests that handle fails when receive a invalid items
    /// </summary>
    [Fact(DisplayName = "Given an invalid items, When handling request, Then should throw Result.Fail")]
    public async Task Handle_InvalidCommandWithInvalidItems_ReturnsResultFail()
    {
        // Given
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items = []
        };

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().BeEquivalentTo("The sale must have at least one item.");
    }

    /// <summary>
    /// Tests that the command is mapped to sale entity successfully
    /// </summary>
    [Fact(DisplayName = "Given a valid command, When handling request, Then should map command to Sale entity")]
    public async Task Handle_ValidCommandMapSale_ReturnsResultOk()
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
    public async Task Handle_ValidCommandPersistSaleRepository_ReturnsResultOk()
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
    public async Task Handle_ValidCommandMapCreateSaleResult_ReturnsResultOk()
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
        result.IsSuccess.Should().BeTrue();
        result.IsFailed.Should().BeFalse();
        _mapper.Received(1).Map<CreateSaleResult>(sale);
    }

    /// <summary>
    /// Tests that the sale entity is mapped to result successfully with the correct sale number
    /// </summary>
    [Fact(DisplayName = "Given a created sale, When handling request, Then should return mapped CreateSaleResult with sale number")]
    public async Task Handle_ValidCommandMapCreateSaleResultWithSaleNumber_ReturnsResultOk()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items = [new() { ProductId = Guid.NewGuid(), Quantity = 1, Price = 10.0m }]
        };

        var sale = new Sale();
        var expectedResult = SaleHandlerTestData.GenerateCreateSaleResult();

        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(sale));
        _mapper.Map<CreateSaleResult>(sale).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailed.Should().BeFalse();
        result.Value.Number.Should().Be(101010);
        _mapper.Received(1).Map<CreateSaleResult>(sale);
    }
}
