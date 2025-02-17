﻿using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Validators;

/// <summary>
/// Contains unit tests for the CreateSaleItemValidator (sale items/products) class.
/// </summary>
public class CreateSaleItemValidatorTests
{
    private readonly CreateSaleCommandValidator.CreateSaleItemValidator _validator;

    public CreateSaleItemValidatorTests()
    {
        _validator = new CreateSaleCommandValidator.CreateSaleItemValidator();
    }

    /// <summary>
    /// Tests that validation fails when productId is not present.
    /// </summary>
    [Fact(DisplayName = "Given an empty ProductId, When validating, Then should have an error")]
    public void Given_An_Empty_ProductId_When_Validating_Then_Should_Have_Error()
    {
        var item = new CreateSaleItemDto
        {
            ProductId = Guid.Empty,
            Quantity = 1,
            Price = 10.0m
        };

        var result = _validator.TestValidate(item);

        result.ShouldHaveValidationErrorFor(x => x.ProductId);
    }

    /// <summary>
    /// Tests that validation fails when quantity is less than one.
    /// </summary>
    [Fact(DisplayName = "Given a quantity less than one, When validating, Then should have an error")]
    public void Given_A_Quantity_Less_Than_One_When_Validating_Then_Should_Have_Error()
    {
        var item = new CreateSaleItemDto
        {
            ProductId = Guid.NewGuid(),
            Quantity = 0,
            Price = 10.0m
        };

        var result = _validator.TestValidate(item);

        result.ShouldHaveValidationErrorFor(x => x.Quantity);
    }

    /// <summary>
    /// Tests that validation fails when price is less than one.
    /// </summary>
    [Fact(DisplayName = "Given a price less than one, When validating, Then should have an error")]
    public void Given_A_Price_Less_Than_One_When_Validating_Then_Should_Have_Error()
    {
        var item = new CreateSaleItemDto
        {
            ProductId = Guid.NewGuid(),
            Quantity = 1,
            Price = 0m
        };

        var result = _validator.TestValidate(item);

        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    /// <summary>
    /// Tests that validation pass when give a valid command with all fields present.
    /// </summary>
    [Fact(DisplayName = "Given a valid item, When validating, Then should not have any errors")]
    public void Given_A_Valid_Item_When_Validating_Then_Should_Not_Have_Error()
    {
        var item = new CreateSaleItemDto
        {
            ProductId = Guid.NewGuid(),
            Quantity = 1,
            Price = 10.0m
        };

        var result = _validator.TestValidate(item);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
