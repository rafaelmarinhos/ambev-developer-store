using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Validators;

/// <summary>
/// Contains unit tests for the CreateSaleCommandValidator class.
/// </summary>
public class CreateSaleCommandValidatorTests
{
    private readonly CreateSaleCommandValidator _validator;

    public CreateSaleCommandValidatorTests()
    {
        _validator = new CreateSaleCommandValidator();
    }

    /// <summary>
    /// Tests that validation fails when customerId is not present.
    /// </summary>
    [Fact(DisplayName = "Given an invalid CustomerId, When validating, Then should have an error")]
    public void Handle_InvalidCommandCustomerId_ReturnsError()
    {
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.Empty,
            BranchId = Guid.NewGuid(),
            Items = [new() { ProductId = Guid.NewGuid(), Quantity = 1, Price = 10.0m }]
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.CustomerId);
        result.Errors[0].ErrorMessage.Should().BeEquivalentTo("The field Customer is required.");
    }

    /// <summary>
    /// Tests that validation fails when branchId is not present.
    /// </summary>
    [Fact(DisplayName = "Given an invalid BranchId, When validating, Then should have an error")]
    public void Handle_InvalidCommandBranchId_ReturnsError()
    {
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.Empty,
            Items = [new() { ProductId = Guid.NewGuid(), Quantity = 1, Price = 10.0m }]
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.BranchId);
        result.Errors[0].ErrorMessage.Should().BeEquivalentTo("The field Branch is required.");
    }

    /// <summary>
    /// Tests that validation fails when not exists items.
    /// </summary>
    [Fact(DisplayName = "Given an empty item list, When validating, Then should have an error")]
    public void Handle_InvalidCommandEmptyItems_ReturnsError()
    {
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items = []
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Items);
        result.Errors[0].ErrorMessage.Should().BeEquivalentTo("The sale must have at least one item.");
    }

    /// <summary>
    /// Tests that validation pass when give a valid command with all fields present.
    /// </summary>
    [Fact(DisplayName = "Given a valid command, When validating, Then should not have any errors")]
    public void Handle_ValidCommand_ReturnsSuccess()
    {
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items = [new() { ProductId = Guid.NewGuid(), Quantity = 1, Price = 10.0m }]
        };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
