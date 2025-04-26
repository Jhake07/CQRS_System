using CleanArchitectureSystem.Domain;

namespace CleanArchitectureSystem.Application.Contracts.Interface
{
    public interface IAppUserRepository : IGenericRepository<AppUser>
    {
        Task<bool> IsUniqueUsername(string username);
    }
}
