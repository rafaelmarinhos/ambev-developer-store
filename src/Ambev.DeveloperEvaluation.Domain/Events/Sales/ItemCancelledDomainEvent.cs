using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sales;

/// <summary>
/// Event to inform that a item was cancelled in a sale
/// </summary>
public class ItemCancelledDomainEvent : INotification
{
    public Guid SaleId { get; }
    public Guid SaleItemId { get; set; }

    public ItemCancelledDomainEvent(Guid saleId, Guid saleItemId)
    {
        SaleId = saleId;
        SaleItemId = saleItemId;
    }
}