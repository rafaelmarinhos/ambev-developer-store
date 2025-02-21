using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

/// <summary>
/// Handler that will be executed when an order is canceled
/// </summary>
public class SaleCanceledDomainEventHandler : INotificationHandler<SaleCancelledDomainEvent>
{
    private readonly ILogger _logger;

    public SaleCanceledDomainEventHandler(ILogger logger)
    {
        _logger = logger;
    }

    public async Task Handle(SaleCancelledDomainEvent notification, CancellationToken cancellationToken)
    {
        // Here you can use Rebus or MassTransit to send integration events to a MessageBroker

        _logger.LogInformation(
            "{Handler} | Sale canceled | Sale: {SaleId}",
            nameof(ItemCanceledDomainEventHandler),
            notification.SaleId            
        );

        await Task.CompletedTask;
    }
}
