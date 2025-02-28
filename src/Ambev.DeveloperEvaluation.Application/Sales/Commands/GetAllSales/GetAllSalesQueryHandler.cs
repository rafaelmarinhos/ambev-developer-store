using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentResults;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.GetAllSales;

/// <summary>
/// Handler to get all sales
/// </summary>
public class GetAllSalesQueryHandler : IRequestHandler<GetAllSalesQuery, Result<IEnumerable<Sale>>>
{
    private readonly ISaleRepository _saleRepository;

    public GetAllSalesQueryHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<Result<IEnumerable<Sale>>> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
    {
        // Repository operation
        var sales = await _saleRepository.GetAllAsync(cancellationToken);
        return Result.Ok(sales);
    }
}
