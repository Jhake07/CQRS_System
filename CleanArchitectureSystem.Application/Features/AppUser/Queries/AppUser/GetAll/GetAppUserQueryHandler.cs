using AutoMapper;
using CleanArchitectureSystem.Application.Contracts.Interface;
using CleanArchitectureSystem.Application.Contracts.Logging;
using MediatR;

namespace CleanArchitectureSystem.Application.Features.AppUser.Queries.AppUser.GetAll
{
    public class GetAppUserQueryHandler(IMapper mapper, IAppUserRepository appUserRepository, IAppLogger<GetAppUserQueryHandler> logger) : IRequestHandler<GetAppUserQuery, List<AppUserDto>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IAppUserRepository _appUserRepository = appUserRepository;
        private readonly IAppLogger<GetAppUserQueryHandler> _logger = logger;

        public async Task<List<AppUserDto>> Handle(GetAppUserQuery request, CancellationToken cancellationToken)
        {
            // Query the database
            var users = await _appUserRepository.GetAsync();

            // Convert data object to DTO
            var data = _mapper.Map<List<AppUserDto>>(users);

            // Return the list of DTO object
            _logger.LogInformation("User retrieve successfully.");
            return data;
        }
    }
}
