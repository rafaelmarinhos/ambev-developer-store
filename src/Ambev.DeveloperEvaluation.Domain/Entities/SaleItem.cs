using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    public Guid SaleId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public decimal TotalAmount { get; private set; }
}