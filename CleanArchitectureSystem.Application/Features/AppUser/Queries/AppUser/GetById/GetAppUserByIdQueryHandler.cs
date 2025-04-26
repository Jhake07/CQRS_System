using AutoMapper;
using CleanArchitectureSystem.Application.Contracts.Interface;
using CleanArchitectureSystem.Application.Exceptions;
using MediatR;

namespace CleanArchitectureSystem.Application.Features.AppUser.Queries.AppUser.GetById
{
    public class GetAppUserByIdQueryHandler(IMapper mapper, IAppUserRepository appUserRepository) : IRequestHandler<GetAppUserByIdQuery, AppUserDto>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IAppUserRepository _appUserRepository = appUserRepository;

        public async Task<AppUserDto> Handle(GetAppUserByIdQuery request, CancellationToken cancellationToken)
        {
            // Query the database
            var user = await _appUserRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException(nameof(AppUser), request.Id);

            // Convert data objects to DTO
            var data = _mapper.Map<AppUserDto>(user);

            // Return DTO object
            return data;
        }
    }
}
