using FluentResults;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.DeleteSale;

/// <summary>
/// Command for delete a sale by their ID
/// </summary>
public class DeleteSaleCommand : IRequest<Result<DeleteSaleResult>>
{
    /// <summary>
    /// The unique identifier of the sale to delete
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of DeleteSaleCommand
    /// </summary>
    /// <param name="id">The ID of the sale to delete</param>
    public DeleteSaleCommand(Guid id)
    {
        Id = id;
    }
}