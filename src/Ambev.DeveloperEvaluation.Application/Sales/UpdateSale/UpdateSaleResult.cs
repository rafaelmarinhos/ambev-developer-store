namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Represents the response returned after successfully update a sale
/// </summary>
/// <remarks>
/// This response contains the unique identifier and the sequential number of the updated sale,
/// which can be used for subsequent operations or reference.
/// </remarks>
public class UpdateSaleResult
{
    public Guid Id { get; set; }
    public long Number { get; set; }
    public int TotalItems { get; set; }
}
