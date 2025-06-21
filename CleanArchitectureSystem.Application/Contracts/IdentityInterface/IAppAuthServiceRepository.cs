using CleanArchitectureSystem.Application.Models.Identity;
using CleanArchitectureSystem.Application.Response;

namespace CleanArchitectureSystem.Application.Contracts.IdentityInterface
{
    public interface IAppAuthServiceRepository
    {
        Task<AuthResponse> Login(AuthRequest request);
        Task<IdentityResultResponse> Register(RegistrationRequest request);
    }
}
