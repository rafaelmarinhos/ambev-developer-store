using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    private readonly List<SaleItem> _items = [];

    public Sale() { }

    public long Number { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public decimal TotalAmount { get; private set; }
    public decimal Discount { get; private set; }
    public bool Cancelled { get; private set; }
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    public void AddItem(SaleItem item)
    {
        _items.Add(new SaleItem(item.ProductId, item.Quantity, item.Price));
        CalculateTotalDiscount();
    }

    public void AddItem(Guid productId, int quantity, decimal price)
    {
        _items.Add(new SaleItem(productId, quantity, price));
        CalculateTotalDiscount();
    }

    public void CancelItem(Guid productId)
    {
        _items.FirstOrDefault(x => x.ProductId == productId)!.Cancel();
        CalculateTotalDiscount();
    }

    public void UpdateItem(Guid productId, int quantity, decimal price)
    {
        var item = _items.FirstOrDefault(x => x.ProductId == productId)!;
        item.Update(quantity, price);
        CalculateTotalDiscount();
    }

    private void CalculateTotalDiscount()
    {
        Discount = _items.Where(f => !f.IsCanceled).Sum(f => f.Discount);
        CalculateTotalAmount();
    }

    private void CalculateTotalAmount()
    {
        TotalAmount = _items.Where(f => !f.IsCanceled).Sum(f => f.TotalAmount);
    }
}
