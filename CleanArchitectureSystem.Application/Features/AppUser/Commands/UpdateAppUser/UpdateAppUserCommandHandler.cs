using AutoMapper;
using CleanArchitectureSystem.Application.Contracts.Interface;
using CleanArchitectureSystem.Application.Contracts.Logging;
using CleanArchitectureSystem.Application.Exceptions;
using MediatR;

namespace CleanArchitectureSystem.Application.Features.AppUser.Commands.Update
{
    public class UpdateAppUserCommandHandler(IMapper mapper, IAppUserRepository appUserRepository, IAppLogger<UpdateAppUserCommandHandler> logger) : IRequestHandler<UpdateAppUserCommand, Unit>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IAppUserRepository _appUserRepository = appUserRepository;
        private readonly IAppLogger<UpdateAppUserCommandHandler> _logger = logger;

        public async Task<Unit> Handle(UpdateAppUserCommand request, CancellationToken cancellationToken)
        {
            // validate data        
            var validator = new UpdateAppUserCommandValidator(_appUserRepository);
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Count != 0)
            {
                _logger.LogInformation("Validation errors in update request for {0} - {1}", nameof(AppUser), request.Id);
                throw new BadRequestException("Validation failed", validationResult);
            }

            // convert to domain object
            var user = _mapper.Map<Domain.AppUser>(request);

            // update the database
            await _appUserRepository.UpdateAsync(user);

            // return Unit
            throw new NotImplementedException();
        }
    }
}
