using AutoMapper;
using CleanArchitectureSystem.Application.Contracts.Interface;
using CleanArchitectureSystem.Application.Exceptions;
using MediatR;

namespace CleanArchitectureSystem.Application.Features.AppUser.Commands.Create
{
    public class CreateAppUserCommandHandler(IMapper mapper, IAppUserRepository appUserRepository) : IRequestHandler<CreateAppUserCommand, int>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IAppUserRepository _appUserRepository = appUserRepository;

        public async Task<int> Handle(CreateAppUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate data
                var validator = new CreateAppUserCommandValidator(_appUserRepository);
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (validationResult.Errors.Count != 0)
                {
                    //_logger.LogWarning("Validation failed for CreateBatchSerialCommand: {Errors}", validationResult.Errors);
                    throw new BadRequestException("Validation failed", validationResult);
                }

                // Convert to domain object
                var user = _mapper.Map<Domain.AppUser>(request);

                // Add to database
                await _appUserRepository.CreateAsync(user);

                // Return record id
                return user.Id;
            }
            catch (Exception)
            {
                throw;
            }


        }
    }
}
