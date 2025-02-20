using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sales;

/// <summary>
/// Event to inform that a new sale was created
/// </summary>
public class SaleCreatedDomainEvent : INotification
{
    public Guid SaleId { get; }

    public SaleCreatedDomainEvent(Guid saleId)
    {
        SaleId = saleId;
    }
}
