namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// API response model for GetSale operation
/// </summary>
public class GetSaleResponse
{
    public Guid Id { get; set; }
    public long Number { get; set; }
    public int TotalItems { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal Discount { get; set; }
    public IEnumerable<GetSaleItemsResponse> Items { get; set; } = [];
}

/// <summary>
/// API response items model for GetSale operation
/// </summary>
public class GetSaleItemsResponse
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsCanceled { get; set; }
}
