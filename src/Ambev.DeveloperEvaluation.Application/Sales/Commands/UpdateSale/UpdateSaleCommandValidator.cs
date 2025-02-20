using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.UpdateSale;

public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateSaleCommandValidator with defined validation rules.
    /// </summary>    
    public UpdateSaleCommandValidator()
    {
        RuleFor(sale => sale.Id)
            .NotEmpty()
            .WithMessage("The field Id is required.");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("The sale must have at least one item.")
            .Must(items => items.Count != 0).WithMessage("The sale must have at least one valid item.");

        RuleForEach(x => x.Items)
            .SetValidator(new UpdateSaleItemValidator());
    }

    /// <summary>
    /// Initializes a new instance of the UpdateSaleItemValidator with defined validation rules.
    /// </summary>    
    public class UpdateSaleItemValidator : AbstractValidator<UpdateSaleItemDto>
    {
        public UpdateSaleItemValidator()
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
