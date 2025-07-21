using CleanArchitectureSystem.Application.DTO;

namespace CleanArchitectureSystem.Application.Contracts.Identity
{
    public interface IAppUserServiceRepository
    {
        Task<List<AppUserDto>> GetUsers();
        Task<AppUserDto> GetUser(string userId);
        public string UserId { get; }
        //Task<IdentityResultResponse> UpdateUserAccount(UpdateRequest request);
    }
}
