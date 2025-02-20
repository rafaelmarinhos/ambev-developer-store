using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

public class ItemCanceledDomainEventHandler : INotificationHandler<ItemCancelledDomainEvent>
{
    public async Task Handle(ItemCancelledDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"{typeof(ItemCanceledDomainEventHandler)} | Sale item canceled | Sale: {notification.SaleId} | ProductId: {notification.SaleItemId}");
        await Task.CompletedTask;
    }
}