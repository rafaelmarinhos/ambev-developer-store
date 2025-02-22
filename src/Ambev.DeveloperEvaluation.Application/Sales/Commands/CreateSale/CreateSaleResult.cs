﻿namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;

/// <summary>
/// Represents the response returned after successfully creating a new sale.
/// </summary>
/// <remarks>
/// This response contains the unique identifier and the sequential number of the newly created sale,
/// which can be used for subsequent operations or reference.
/// </remarks>
public class CreateSaleResult
{
    public Guid Id { get; set; }
    public long Number { get; set; }
    public int TotalItems { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal Discount { get; set; }
}