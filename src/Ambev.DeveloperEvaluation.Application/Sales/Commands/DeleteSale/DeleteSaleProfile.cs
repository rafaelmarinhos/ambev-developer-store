using Ambev.DeveloperEvaluation.Application.Sales.Commands.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.DeleteSale;

/// <summary>
/// Profile for mapping between Sale and DeleteSaleResult
/// </summary>
public class DeleteSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateSale operation
    /// </summary>
    public DeleteSaleProfile()
    {
        CreateMap<Sale, DeleteSaleResult>();
    }
}