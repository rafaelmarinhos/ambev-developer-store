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
    public decimal Discount { get; private set; }

    public SaleItem(Guid productId, int quantity, decimal price)
    {
        ProductId = productId;
        Quantity = quantity;
        Price = price;
        Discount = ApplyDiscount();
        CalculateTotalAmount();
    }

    public void Update(int quantity, decimal price)
    {
        Quantity = quantity;
        Price = price;
        UpdatedAt = DateTime.UtcNow;
        Discount = ApplyDiscount();
        CalculateTotalAmount();
    }

    private decimal ApplyDiscount()
    {
        // No discounts allowed for quantities below 4 items
        if (Quantity < 4)
            return 0;

        // 4+ items: 10% discount
        if (Quantity < 10)
            return Quantity * Price * 0.10m;

        // 11 - 20 items: 20% discount
        return Quantity * Price * 0.20m;        
    }

    public void CalculateTotalAmount()
    {
        TotalAmount = (Quantity * Price);
    }

    public void Cancel()
    {
        IsCanceled = true;
        UpdatedAt = DateTime.UtcNow;
    }
}