using CleanArchitectureSystem.Application.Models.Identity;
using CleanArchitectureSystem.Application.Response;

namespace CleanArchitectureSystem.Application.Contracts.IdentityInterface
{
    public interface IAppUserServiceRepository
    {
        Task<List<User>> GetUsers();
        Task<User> GetUser(string userId);
        public string UserId { get; }
        Task<IdentityResultResponse> UpdateUserAccount(UpdateRequest request);
    }
}
