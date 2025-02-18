using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleCommand that defines validation rules for sale creation command.
/// </summary>
public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - CustomerId: Required
    /// - BranchId: Required
    /// </remarks>
    public CreateSaleCommandValidator()
    {
        RuleFor(sale => sale.CustomerId)
            .NotEmpty()
            .WithMessage("The field Customer is required.");

        RuleFor(sale => sale.BranchId)
            .NotEmpty()
            .WithMessage("The field Branch is required.");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("The sale must have at least one item.")
            .Must(items => items.Count != 0).WithMessage("The sale must have at least one valid item.");

        RuleForEach(x => x.Items)
            .SetValidator(new CreateSaleItemValidator());
    }

    /// <summary>
    /// Initializes a new instance of the CreateSaleItemValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - CustomerId: Required
    /// - BranchId: Required
    /// </remarks>
    public class CreateSaleItemValidator : AbstractValidator<CreateSaleItemDto>
    {
        public CreateSaleItemValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("The field Product is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity of this item must be greater than zero.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Price of this item must be greater than zero.");
        }
    }
}