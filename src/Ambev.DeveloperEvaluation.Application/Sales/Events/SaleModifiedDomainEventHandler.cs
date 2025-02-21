using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

/// <summary>
/// Handler that will be executed when an order is modified
/// </summary>
public class SaleModifiedDomainEventHandler : INotificationHandler<SaleModifiedDomainEvent>
{
    private readonly ILogger _logger;

    public SaleModifiedDomainEventHandler(ILogger logger)
    {
        _logger = logger;
    }

    public async Task Handle(SaleModifiedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Here you can use Rebus or MassTransit to send integration events to a MessageBroker

        _logger.LogInformation(
            "{Handler} | Sale modified | Sale: {SaleId}",
            nameof(ItemCanceledDomainEventHandler),
            notification.SaleId
        );

        await Task.CompletedTask;
    }
}
