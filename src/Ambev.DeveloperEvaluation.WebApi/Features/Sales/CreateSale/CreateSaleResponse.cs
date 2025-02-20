namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// API response model for CreateSale operation
/// </summary>
public class CreateSaleResponse
{
    public Guid Id { get; set; }
    public long Number { get; set; }
    public int TotalItems { get; set; }
    public decimal TotalAmount { get; set; }    
}