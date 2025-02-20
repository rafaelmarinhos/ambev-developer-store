using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

/// <summary>
/// Handler that will be executed when an order is created
/// </summary>
public class SaleCreatedDomainEventHandler : INotificationHandler<SaleCreatedDomainEvent>
{
    public async Task Handle(SaleCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"{typeof(SaleCreatedDomainEventHandler)} | Sale created | {notification.SaleId}");
        await Task.CompletedTask;
    }
}