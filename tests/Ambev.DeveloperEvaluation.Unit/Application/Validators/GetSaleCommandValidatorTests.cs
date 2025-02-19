using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Validators;

/// <summary>
/// Contains unit tests for the GetSaleCommandValidator class.
/// </summary>
public class GetSaleCommandValidatorTests
{
    private readonly GetSaleCommandValidator _validator;

    public GetSaleCommandValidatorTests()
    {
        _validator = new GetSaleCommandValidator();
    }

    /// <summary>
    /// Tests that validation fails when Id is not present.
    /// </summary>
    [Fact(DisplayName = "Given an invalid Id, When validating, Then should have an error")]
    public void Handle_InvalidCommand_ReturnsError()
    {
        var command = new GetSaleCommand(Guid.Empty);

        var result = _validator.TestValidate(command);

        result.Errors.Count.Equals(0);
        result.ShouldHaveValidationErrorFor(x => x.Id);
        result.Errors[0].ErrorMessage.Should().BeEquivalentTo("Sale ID is required.");
    }

    /// <summary>
    /// Tests that validation pass when give a valid command with all fields present.
    /// </summary>
    [Fact(DisplayName = "Given a valid command, When validating, Then should not have any errors")]
    public void Handle_ValidCommand_ReturnsSuccess()
    {
        var command = new GetSaleCommand(Guid.NewGuid());

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
