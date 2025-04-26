using CleanArchitectureSystem.Domain;
using Microsoft.EntityFrameworkCore.Storage;

namespace CleanArchitectureSystem.Application.Contracts.Interface
{
    public interface IBatchSerialRepository : IGenericRepository<BatchSerial>
    {
        Task<List<BatchSerial>> GetBatchSerials();
        Task<BatchSerial> GetBatchSerialsById(int id);
        Task<BatchSerial> GetBatchSerialsByContractNo(string contractNo);
        Task<bool> CheckBatchContractNo(string contractNo);
        Task<bool> CheckMainSerialPrefix(string serialPrefix);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
