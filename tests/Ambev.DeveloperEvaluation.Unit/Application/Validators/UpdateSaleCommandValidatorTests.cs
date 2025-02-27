﻿using Ambev.DeveloperEvaluation.Application.Sales.Commands.UpdateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;
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
    public void Handle_InvalidCommandId_ReturnsError()
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
    public void Handle_InvalidCommandListItems_ReturnsError()
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
    public void Handle_ValidCommand_ReturnsSuccess()
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
