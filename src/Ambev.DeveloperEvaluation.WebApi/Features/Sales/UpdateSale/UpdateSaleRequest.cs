namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public class UpdateSaleRequest
{
    public Guid Id { get; set; }    
    public IEnumerable<UpdateSaleItemRequest> Items { get; set; } = [];
}

public class UpdateSaleItemRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}