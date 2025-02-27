﻿namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.UpdateSale;

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
    public decimal TotalAmount { get; set; }
    public decimal Discount { get; set; }
    public IEnumerable<UpdateSaleItemResult> Items { get; set; } = [];
}

/// <summary>
/// Items model for GetSale operation
/// </summary>
public class UpdateSaleItemResult
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsCanceled { get; set; }
}