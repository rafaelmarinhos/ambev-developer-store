using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

/// <summary>
/// Handler that will be executed when an order is created
/// </summary>
public class SaleCreatedDomainEventHandler : INotificationHandler<SaleCreatedDomainEvent>
{
    private readonly ILogger<SaleCreatedDomainEventHandler> _logger;

    public SaleCreatedDomainEventHandler(ILogger<SaleCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(SaleCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Here you can use Rebus or MassTransit to send integration events to a MessageBroker

        _logger.LogInformation(
            "{Handler} | Sale created | Sale: {SaleId}",
            nameof(SaleCreatedDomainEventHandler),
            notification.SaleId
        );

        await Task.CompletedTask;
    }
}