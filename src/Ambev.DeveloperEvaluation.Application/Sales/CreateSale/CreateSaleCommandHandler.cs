using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentResults;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests
/// </summary>
public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, Result<CreateSaleResult>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of CreateSaleCommandHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public CreateSaleCommandHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the CreateSaleCommand request
    /// </summary>
    /// <param name="command">The CreateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    public async Task<Result<CreateSaleResult>> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        // Validate command
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return Result.Fail(errors);
        }

        // Map command to entity
        var sale = _mapper.Map<Sale>(command);

        // Add itens do sale
        foreach (var item in command.Items)
        {
            // Maximum limit: 20 items per product
            if (item.Quantity > 20)
            {
                return Result.Fail("It's not possible to add above 20 identical items.");
            }

            sale.AddItem(item.ProductId, item.Quantity, item.Price);
        }

        // Repository operation
        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

        // Map created sale to result and return
        var result = _mapper.Map<CreateSaleResult>(createdSale);

        return Result.Ok(result);
    }
}
