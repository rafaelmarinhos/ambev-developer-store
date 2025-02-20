using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sales;

/// <summary>
/// Event to inform that a sale was updated
/// </summary>
public class SaleModifiedDomainEvent : INotification
{
    public Guid SaleId { get; }

    public SaleModifiedDomainEvent(Guid saleId)
    {
        SaleId = saleId;
    }
}