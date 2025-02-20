using Ambev.DeveloperEvaluation.Application.Sales.Commands.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Handlers;

/// <summary>
/// Contains unit tests for the <see cref="GetSaleCommandHandler"/> class.
/// </summary>
public class GetSaleCommandHandlerUnitTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSaleCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSaleCommandHandlerUnitTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public GetSaleCommandHandlerUnitTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetSaleCommandHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that handle fails when receive a invalid Id
    /// </summary>
    [Fact(DisplayName = "Given an invalid Id, When handling request, Then should throw Result.Fail")]
    public async Task Handle_InvalidCommandInvalidId_ReturnsResultFail()
    {
        // Given
        var command = new GetSaleCommand(Guid.Empty);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().BeEquivalentTo("Sale ID is required.");
    }

    /// <summary>
    /// Tests that handle fails when sale is not found by Id
    /// </summary>
    [Fact(DisplayName = "Given an non-existent Id, When handling request, Then should throw Result.Fail")]
    public async Task Handle_InvalidCommandNotExistentId_ReturnsResultFail()
    {
        // Given
        var id = Guid.NewGuid();
        var command = new GetSaleCommand(id);

        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult<Sale?>(null));

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().BeEquivalentTo($"Sale with ID {id} not found.");
    }

    /// <summary>
    /// Tests that the sale is mapped to result successfully
    /// </summary>
    [Fact(DisplayName = "Given a valid Id, When handling request, Then should map Sale to Result and return Result.Ok")]
    public async Task Handle_ValidCommandMapSaleToResult_ReturnsResultOk()
    {
        // Given
        var command = new GetSaleCommand(Guid.NewGuid());

        var fakeSale = SaleHandlerTestData.GenerateSale();
        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(fakeSale);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<GetSaleResult>(fakeSale);
        result.IsSuccess.Should().BeTrue();
        result.IsFailed.Should().BeFalse();
    }
}
