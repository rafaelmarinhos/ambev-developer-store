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

    public void AddItem(Guid productId, int quantity, decimal price)
    {
        // TODO: validar se o produto já existe
        _items.Add(new SaleItem(productId, quantity, price));
    }

    public void CancelItem(Guid productId)
    {
        _items.FirstOrDefault(x => x.ProductId == productId)!.Cancel();
    }
}
