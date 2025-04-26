using CleanArchitectureSystem.Application.Contracts.Interface;
using CleanArchitectureSystem.Domain;
using CleanArchitectureSystem.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureSystem.Persistence.Repositories
{
    public class AppUserRepository(CleanArchDbContext context) : GenericRepository<AppUser>(context), IAppUserRepository
    {
        public async Task<bool> IsUniqueUsername(string username)
        {
            return await _context.AppUsers.AnyAsync(u => u.Username == username);
        }
    }

}
