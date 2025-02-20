using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

/// <summary>
/// Handler that will be executed when an order is modified
/// </summary>
public class SaleModifiedDomainEventHandler : INotificationHandler<SaleModifiedDomainEvent>
{
    public async Task Handle(SaleModifiedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"{typeof(SaleModifiedDomainEventHandler)} | Sale modified | {notification.SaleId}");
        await Task.CompletedTask;
    }
}
