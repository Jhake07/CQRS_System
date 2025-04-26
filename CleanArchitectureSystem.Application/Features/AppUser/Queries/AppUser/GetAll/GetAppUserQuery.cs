using MediatR;

namespace CleanArchitectureSystem.Application.Features.AppUser.Queries.AppUser.GetAll
{
    //public class GetAppUserQuery : IRequest<List<AppUserDto>>{}
    public record GetAppUserQuery : IRequest<List<AppUserDto>> { }
}
