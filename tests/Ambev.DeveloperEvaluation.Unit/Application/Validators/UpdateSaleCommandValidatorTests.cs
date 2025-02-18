using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Validators;

/// <summary>
/// Contains unit tests for the UpdateSaleCommandValidator class.
/// </summary>
public class UpdateSaleCommandValidatorTests
{
    private readonly UpdateSaleCommandValidator _validator;

    public UpdateSaleCommandValidatorTests()
    {
        _validator = new UpdateSaleCommandValidator();
    }

    /// <summary>
    /// Tests that validation fails when Id is not present.
    /// </summary>
    [Fact(DisplayName = "Given an invalid Id, When validating, Then should have an error")]
    public void Given_An_Invalid_Id_When_Validating_Then_Should_Have_Error()
    {
        var command = new UpdateSaleCommand()
        {
            Id = Guid.Empty,
            Items = []
        };

        var result = _validator.TestValidate(command);

        result.Errors.Count.Equals(0);
        result.ShouldHaveValidationErrorFor(x => x.Id);
        result.Errors[0].ErrorMessage.Should().BeEquivalentTo("The field Id is required.");
    }

    /// <summary>
    /// Tests that validation fails when not exists items.
    /// </summary>
    [Fact(DisplayName = "Given an empty item list, When validating, Then should have an error")]
    public void Given_An_Empty_Item_List_When_Validating_Then_Should_Have_Error()
    {
        var command = new UpdateSaleCommand
        {
            Id = Guid.NewGuid(),
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
    public void Given_A_Valid_Command_When_Validating_Then_Should_Not_Have_Error()
    {
        var command = new UpdateSaleCommand
        {
            Id = Guid.NewGuid(),
            Items = [new() { ProductId = Guid.NewGuid(), Quantity = 1, Price = 10.0m }]
        };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
