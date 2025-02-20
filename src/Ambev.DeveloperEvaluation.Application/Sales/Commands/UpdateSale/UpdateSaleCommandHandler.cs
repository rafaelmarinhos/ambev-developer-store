using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentResults;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.UpdateSale;

/// <summary>
/// Handler for processing UpdateSaleCommand requests
/// </summary>
public class UpdateSaleCommandHandler : IRequestHandler<UpdateSaleCommand, Result<UpdateSaleResult>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of UpdateSaleCommandHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public UpdateSaleCommandHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the UpdateSaleCommand request
    /// </summary>
    /// <param name="command">The UpdatedSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale details</returns>
    public async Task<Result<UpdateSaleResult>> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        // Validate command
        var validator = new UpdateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return Result.Fail(errors);
        }

        // Repository operation
        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);

        // Validated if sale exists
        if (sale == null)
        {
            return Result.Fail($"Sale with ID {command.Id} not found.");
        }

        // Operations with items
        foreach (var commandItem in command.Items)
        {
            // Maximum limit: 20 items per product
            if (commandItem.Quantity > 20)
            {
                return Result.Fail("It's not possible to add above 20 identical items.");
            }

            var saleItem = sale.Items.FirstOrDefault(f => f.ProductId == commandItem.ProductId);

            // If item not exists, then add
            if (saleItem is null)
            {
                sale.AddItem(commandItem.ProductId, commandItem.Quantity, commandItem.Price);
                continue;
            }

            // If item exists and is canceled, then cancel item
            if (commandItem.IsCanceled)
            {
                sale.CancelItem(commandItem.ProductId);
                continue;
            }

            // If item exists and is not canceled, then just update
            sale.UpdateItem(commandItem.ProductId, commandItem.Quantity, commandItem.Price);
        }

        // Repository operation
        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        // Map updated sale to result and return
        var result = _mapper.Map<UpdateSaleResult>(updatedSale);

        return Result.Ok(result);
    }
}
