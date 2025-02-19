namespace Ambev.DeveloperEvaluation.Application.Sales;

/// <summary>
/// DTO for sale items in a SaleCommandS.
/// </summary>
public class SaleItemDTO
{
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the price of the product at the time of sale.
    /// </summary>
    public decimal Price { get; set; }   
}
