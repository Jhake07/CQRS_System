using CleanArchitectureSystem.Application.Models.Identity;
using CleanArchitectureSystem.Application.Response;

namespace CleanArchitectureSystem.Application.Contracts.Identity
{
    public interface IAppAuthServiceRepository
    {
        Task<AuthResponse> Login(AuthRequest request);
        Task<IdentityResultResponse> Register(RegistrationRequest request, CancellationToken cancellationToken);
        Task<CustomResultResponse> UpdateUserStatus(UpdateUserStatusRequest request);
        Task<CustomResultResponse> UpdateUserCredentials(UpdateUserCredentialsRequest request);
        Task<CustomResultResponse> ResetUserCredentials(ResetPasswordRequest request);
    }
}
