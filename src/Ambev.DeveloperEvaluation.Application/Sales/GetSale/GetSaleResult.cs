namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Result model for GetSale operation
/// </summary>
public class GetSaleResult
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public long Number { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal Discount { get; set; }
    public bool Cancelled { get; set; }
    public IEnumerable<GetSaleItemsResult> Items { get; set; } = [];
}
