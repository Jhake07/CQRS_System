using CleanArchitectureSystem.Application.Models.Identity;
using FluentValidation;

namespace CleanArchitectureSystem.Identity.Services
{
    public class AppAuthServiceValidatorUpdateAccount : AbstractValidator<UpdateRequest>
    {
        public AppAuthServiceValidatorUpdateAccount()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(4).WithMessage("Username must be at least 4 characters.")
                .MaximumLength(30).WithMessage("Username must not exceed 30 characters.");

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("IsActive flag must be provided.");

            RuleFor(x => x.Password)
             .NotEmpty().WithMessage("Password is required.")
             .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Passwords do not match.");
        }
    }
}
