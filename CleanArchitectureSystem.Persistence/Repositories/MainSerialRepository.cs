﻿using CleanArchitectureSystem.Application.Contracts.Interface;
using CleanArchitectureSystem.Domain;
using CleanArchitectureSystem.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureSystem.Persistence.Repositories
{
    public class MainSerialRepository(CleanArchDbContext context) : GenericRepository<MainSerial>(context), IMainSerialRepository
    {
        public async Task BulkCreateAsync(IEnumerable<MainSerial> mainSerials, CancellationToken cancellationToken)
        {
            try
            {
                // Use EF Core's `AddRangeAsync` to add multiple entities in a single operation
                await _context.Set<Domain.MainSerial>().AddRangeAsync(mainSerials, cancellationToken);

                // Save changes to persist the records in the database
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                // Log error and rethrow or handle appropriately
                Console.WriteLine($"An error occurred during bulk insert: {ex.Message}");
                throw;
            }

        }
        public async Task UpdateContractNoAsync(string oldContractNo, string newContractNo)
        {
            // Retrieve all MainSerial records with the old contract number
            var mainSerialRecords = await _context.MainSerials
                .Where(m => m.BatchSerial_ContractNo == oldContractNo)
                .ToListAsync();

            // Check if any records were found
            if (!mainSerialRecords.Any())
            {
                return;
            }

            // Update all records with the new contract number
            mainSerialRecords.ForEach(m => m.BatchSerial_ContractNo = newContractNo);

            // Save all changes to the database
            await _context.SaveChangesAsync();
        }

    }
}
