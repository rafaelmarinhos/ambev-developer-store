using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sales;

/// <summary>
/// Event to inform that a sale was cancelled
/// </summary>
public class SaleCancelledDomainEvent : INotification
{
    public Guid SaleId { get; }

    public SaleCancelledDomainEvent(Guid saleId)
    {
        SaleId = saleId;
    }
}