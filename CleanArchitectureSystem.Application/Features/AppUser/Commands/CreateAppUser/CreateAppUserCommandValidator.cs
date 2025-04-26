using CleanArchitectureSystem.Application.Contracts.Interface;
using FluentValidation;

namespace CleanArchitectureSystem.Application.Features.AppUser.Commands.Create
{
    public class CreateAppUserCommandValidator : AbstractValidator<CreateAppUserCommand>
    {
        private readonly IAppUserRepository _appUserRepository;

        public CreateAppUserCommandValidator(IAppUserRepository appUserRepository)
        {
            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("{Propertyname} is required.")
                .NotNull()
                .MaximumLength(15).WithMessage("{PropertName} maximum 15 characters only.");

            RuleFor(q => q)
                .MustAsync(UniqueUsername).WithMessage("Username already exists.");

            _appUserRepository = appUserRepository;
        }

        private Task<bool> UniqueUsername(CreateAppUserCommand command, CancellationToken cancellationToken)
        {
            return _appUserRepository.IsUniqueUsername(command.Username);
        }
    }
}
