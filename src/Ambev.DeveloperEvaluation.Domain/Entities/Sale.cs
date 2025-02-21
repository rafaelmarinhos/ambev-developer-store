using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    private readonly List<SaleItem> _items = [];

    public Sale() { }

    public long Number { get; set; }
    public Guid CustomerId { get; private set; }
    public Guid BranchId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public decimal Discount { get; private set; }
    public bool Cancelled { get; private set; }
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    public Sale(Guid customerId, Guid branchId)
    {
        CustomerId = customerId;
        BranchId = branchId;

        // Create domain event to inform that a new sale was created
        AddDomainEvent(new SaleCreatedDomainEvent(Id));
    }

    public void AddItem(Guid productId, int quantity, decimal price)
    {
        _items.Add(new SaleItem(productId, quantity, price));
    }

    public void CancelItem(Guid productId)
    {
        _items.FirstOrDefault(x => x.ProductId == productId)!.Cancel();

        // Create domain event to inform that a new item was added
        AddDomainEvent(new ItemCancelledDomainEvent(Id, productId));
    }

    public void UpdateItem(Guid productId, int quantity, decimal price)
    {
        var item = _items.FirstOrDefault(x => x.ProductId == productId)!;
        item.Update(quantity, price);
    }

    public void Cancel()
    {
        Cancelled = true;

        // Create domain evento to inform that a sale was canceled
        AddDomainEvent(new SaleCancelledDomainEvent(Id));
    }

    public void CalculateTotals()
    {
        CalculateTotalDiscount();
        CalculateTotalAmount();

        // Create domain event to inform that a sale as updated
        AddDomainEvent(new SaleModifiedDomainEvent(Id));
    }

    private void CalculateTotalDiscount()
    {
        Discount = _items.Where(f => !f.IsCanceled).Sum(f => f.Discount);        
    }

    private void CalculateTotalAmount()
    {
        TotalAmount = _items.Where(f => !f.IsCanceled).Sum(f => f.TotalAmount);
    }
}
