using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    public Guid SaleId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public decimal TotalAmount { get; private set; }
    public bool IsCanceled { get; private set; } = false;

    public SaleItem(Guid productId, int quantity, decimal price)
    {
        ProductId = productId;
        Quantity = quantity;
        Price = price;
        TotalAmount = quantity * price;
        IsCanceled = false;
    }

    public void Cancel() => IsCanceled = true;

    public void Update(int quantity, decimal price)
    {
        Quantity = quantity;
        Price = price;
    }
}