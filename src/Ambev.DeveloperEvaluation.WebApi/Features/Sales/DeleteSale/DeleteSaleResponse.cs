namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;

/// <summary>
/// API response model for DeleleSale operation
/// </summary>
public class DeleteSaleResponse
{
    public Guid Id { get; set; }
    public long Number { get; set; }
    public bool Cancelled { get; set; }
}
