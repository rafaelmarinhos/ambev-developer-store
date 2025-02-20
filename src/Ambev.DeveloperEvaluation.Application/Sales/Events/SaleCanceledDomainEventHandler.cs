using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

public class SaleCanceledDomainEventHandler : INotificationHandler<SaleCancelledDomainEvent>
{
    public async Task Handle(SaleCancelledDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"{typeof(SaleCanceledDomainEventHandler)} | Sale cancelled | {notification.SaleId}");
        await Task.CompletedTask;
    }
}
