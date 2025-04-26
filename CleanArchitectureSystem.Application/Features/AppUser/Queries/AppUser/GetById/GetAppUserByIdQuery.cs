using MediatR;

namespace CleanArchitectureSystem.Application.Features.AppUser.Queries.AppUser.GetById
{
    public record GetAppUserByIdQuery(int Id) : IRequest<AppUserDto> { }
}
