
/// <summary>
/// Represents a request to create a new sale in the system.
/// </summary>
namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleRequest
{
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public IEnumerable<CreateSaleItemRequest> Items { get; set; } = [];
}

public class CreateSaleItemRequest
{        
    public Guid ProductId { get; set; }        
    public int Quantity { get; set; }        
    public decimal Price { get; set; }
}