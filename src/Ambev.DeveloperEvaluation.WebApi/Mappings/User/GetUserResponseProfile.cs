using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.User;

public class GetUserResponseProfile : Profile
{
    public GetUserResponseProfile()
    {
        CreateMap<GetUserResult, GetUserResponse>();
    }
}