namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;


/// <summary>
/// Request model for delete a sale by ID
/// </summary>
public class DeleteSaleRequest
{
    /// <summary>
    /// The unique identifier of the sale to retrieve
    /// </summary>
    public Guid Id { get; set; }
}