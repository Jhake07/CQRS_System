using CleanArchitectureSystem.Application.Contracts.Interface;
using CleanArchitectureSystem.Domain;
using CleanArchitectureSystem.Persistence.DatabaseContext;

namespace CleanArchitectureSystem.Persistence.Repositories
{
    public class ItemRepository(CleanArchDbContext context) : GenericRepository<Item>(context), IItemRepository
    {

    }

}
