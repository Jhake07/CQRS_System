using CleanArchitectureSystem.Application.Contracts.Interface;
using CleanArchitectureSystem.Application.Exceptions;
using MediatR;

namespace CleanArchitectureSystem.Application.Features.AppUser.Commands.Delete
{
    public class DeleteAppUserCommandHandler(IAppUserRepository appUserRepository) : IRequestHandler<DeleteAppUserCommand, Unit>
    {
        private readonly IAppUserRepository _appUserRepository = appUserRepository;

        public async Task<Unit> Handle(DeleteAppUserCommand request, CancellationToken cancellationToken)
        {
            /// old way            
            ////var user = await _appUserRepository.GetByIdAsync(request.Id);           
            ////if (user == null)
            ////{
            ////    throw new NotFoundException(nameof(AppUser), request.Id);
            ////}

            // retrieve domain object and 
            var user = await _appUserRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException(nameof(AppUser), request.Id);

            // remove from the database
            await _appUserRepository.DeleteAsync(user);

            return Unit.Value;
        }
    }
}
