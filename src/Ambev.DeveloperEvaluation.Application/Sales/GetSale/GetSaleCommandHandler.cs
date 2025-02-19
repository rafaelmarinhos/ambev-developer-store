using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentResults;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Handler for processing GetSaleCommand requests
/// </summary>
public class GetSaleCommandHandler : IRequestHandler<GetSaleCommand, Result<GetSaleResult>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetSaleCommandHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>    
    public GetSaleCommandHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<Result<GetSaleResult>> Handle(GetSaleCommand command, CancellationToken cancellationToken)
    {
        // Validate command
        var validator = new GetSaleCommandValidator();
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

        // Map found sale to result
        var result = _mapper.Map<GetSaleResult>(sale);

        return Result.Ok(result);
    }
}