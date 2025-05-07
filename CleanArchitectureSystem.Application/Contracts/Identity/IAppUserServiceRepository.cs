using CleanArchitectureSystem.Application.Models.Identity;

namespace CleanArchitectureSystem.Application.Contracts.Identity
{
    public interface IAppUserServiceRepository
    {
        Task<List<User>> GetUsers();
        Task<User> GetUser(string userId);
        public string UserId { get; }
    }
}
