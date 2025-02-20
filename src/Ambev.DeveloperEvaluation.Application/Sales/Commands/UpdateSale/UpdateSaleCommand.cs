using FluentResults;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.UpdateSale;

/// <summary>
/// Command for updating a sale.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for updating a sale.
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="UpdateSaleResult"/>.
/// 
/// The data provided in this command is validated using the 
/// <see cref="UpdateSaleValidator"/> which extends 
/// <see cref="AbstractValidator{T}"/> to ensure that the fields are correctly 
/// populated and follow the required rules.
/// </remarks>
public class UpdateSaleCommand : IRequest<Result<UpdateSaleResult>>
{
    /// <summary>
    /// Gets or sets the Id of the sale to be updated
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the list of sale items.
    /// </summary>
    public List<UpdateSaleItemDto> Items { get; set; } = [];
}

/// <summary>
/// DTO for sale items in a UpdateSaleCommand.
/// </summary>
public class UpdateSaleItemDto : SaleItemDTO
{
    /// <summary>
    /// Gets or sets the status of the product: Canceled/Not Canceled
    /// </summary>
    public bool IsCanceled { get; set; } = false;
}
