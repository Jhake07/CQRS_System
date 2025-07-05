using CleanArchitectureSystem.Application.Models.Identity;
using CleanArchitectureSystem.Application.Response;

namespace CleanArchitectureSystem.Application.Contracts.Identity
{
    public interface IAppAuthServiceRepository
    {
        Task<AuthResponse> Login(AuthRequest request);
        Task<IdentityResultResponse> Register(RegistrationRequest request, CancellationToken cancellationToken);
        Task<CustomResultResponse> UpdateUserAccount(string username, string email, UpdateRequest request);
    }
}
