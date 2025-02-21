using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentResults;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.DeleteSale;

/// <summary>
/// Handler for processing DeleteSaleCommand requests
/// </summary>
public class DeleteSaleCommandHandler : IRequestHandler<DeleteSaleCommand, Result<DeleteSaleResult>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of DeleteSaleCommandHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public DeleteSaleCommandHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<Result<DeleteSaleResult>> Handle(DeleteSaleCommand command, CancellationToken cancellationToken)
    {
        // Validate command
        var validator = new DeleteSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return Result.Fail(errors);
        }

        // Try to retrieve sale by id
        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);

        // Validated if sale exists
        if (sale == null)
        {
            return Result.Fail($"Sale with ID {command.Id} not found.");
        }

        // Repository operation
        sale.Cancel();
        var deletedSale = await _saleRepository.CancelAsync(sale, cancellationToken);

        // Map found sale to result
        var result = _mapper.Map<DeleteSaleResult>(sale);

        return Result.Ok(result);
    }
}
