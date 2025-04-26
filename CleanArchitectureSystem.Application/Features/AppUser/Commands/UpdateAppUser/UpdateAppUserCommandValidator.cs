using CleanArchitectureSystem.Application.Contracts.Interface;
using FluentValidation;

namespace CleanArchitectureSystem.Application.Features.AppUser.Commands.Update
{
    public class UpdateAppUserCommandValidator : AbstractValidator<UpdateAppUserCommand>
    {
        private readonly IAppUserRepository _appUserRepository;
        public UpdateAppUserCommandValidator(IAppUserRepository appUserRepository)
        {
            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("{Propertyname} is required.")
                .NotNull()
                .MaximumLength(15).WithMessage("{PropertName} maximum 15 characters only.");

            RuleFor(q => q)
                .MustAsync(UniqueUsername).WithMessage("Username already exists.");

            _appUserRepository = appUserRepository;
        }

        private Task<bool> UniqueUsername(UpdateAppUserCommand command, CancellationToken cancellationToken)
        {
            return _appUserRepository.IsUniqueUsername(command.Username);
        }
    }
}
