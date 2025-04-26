using CleanArchitectureSystem.Domain;

namespace CleanArchitectureSystem.Application.Contracts.Interface
{
    public interface IMainSerialRepository : IGenericRepository<MainSerial>
    {
        //Task CreateMainSerials(MainSerial mainSerial);
        Task BulkCreateAsync(IEnumerable<Domain.MainSerial> mainSerials, CancellationToken cancellationToken);
        Task UpdateContractNoAsync(string oldContractNo, string newContractNo);
    }
}
