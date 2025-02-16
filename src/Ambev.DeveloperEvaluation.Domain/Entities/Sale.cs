﻿using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    private readonly List<SaleItem> _items = [];

    public Sale() { }

    public long Number { get; set; }
    public Guid CustomerId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public Guid BranchId { get; private set; }
    public decimal Discount { get; private set; }
    public bool Cancelled { get; private set; }
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();
}
