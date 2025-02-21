using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

/// <summary>
/// Handler that will be executed when an order item is canceled
/// </summary>
public class ItemCanceledDomainEventHandler : INotificationHandler<ItemCancelledDomainEvent>
{
    private readonly ILogger<ItemCanceledDomainEventHandler> _logger;

    public ItemCanceledDomainEventHandler(ILogger<ItemCanceledDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(ItemCancelledDomainEvent notification, CancellationToken cancellationToken)
    {
        // Here you can use Rebus or MassTransit to send integration events to a MessageBroker

        _logger.LogInformation(
            "{Handler} | Sale item canceled | Sale: {SaleId} | ProductId: {SaleItemId}",
            nameof(ItemCanceledDomainEventHandler),
            notification.SaleId,
            notification.SaleItemId
        );

        await Task.CompletedTask;
    }
}