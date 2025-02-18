namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Items model for GetSale operation
/// </summary>
public class GetSaleItemsResult
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalAmount { get; set; }
}
