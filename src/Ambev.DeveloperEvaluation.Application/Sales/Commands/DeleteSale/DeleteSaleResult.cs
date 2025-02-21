namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.DeleteSale;

/// <summary>
/// Result model for DeleteSale operation
/// </summary>
public class DeleteSaleResult
{
    public Guid Id { get; set; }
    public long Number { get; set; }
    public bool Cancelled { get; set; }
}
