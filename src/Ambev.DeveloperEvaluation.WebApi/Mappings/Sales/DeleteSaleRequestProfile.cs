using Ambev.DeveloperEvaluation.Application.Sales.Commands.DeleteSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.Sales;

public class DeleteSaleRequestProfile : Profile
{
    public DeleteSaleRequestProfile()
    {
        CreateMap<DeleteSaleRequest, DeleteSaleCommand>();
        CreateMap<DeleteSaleResult, DeleteSaleResponse>();
    }
}
