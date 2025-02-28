using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentResults;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.GetAllSales;

public class GetAllSalesQuery : IRequest<Result<IEnumerable<Sale>>> { }
