using CleanArchitectureSystem.Application.Models.Identity;
using CleanArchitectureSystem.Application.Response;

namespace CleanArchitectureSystem.Application.Contracts.Identity
{
    public interface IAppAuthServiceRepository
    {
        Task<AuthResponse> Login(AuthRequest request);
        Task<RegistrationResponse> Register(RegistrationRequest request);
        Task<CustomResultResponse> UpdateUserAccount(UpdateRequest request);
    }
}
