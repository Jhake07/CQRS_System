﻿using AutoMapper;
using CleanArchitectureSystem.Application.Contracts.Interface;
using CleanArchitectureSystem.Application.Exceptions;
using CleanArchitectureSystem.Application.Response;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitectureSystem.Application.Features.BatchSerial.Commands.UpdateBatchSerials
{
    public class UpdateBatchSerialCommandHandler(
        IMapper mapper,
        ILogger<UpdateBatchSerialCommandHandler> logger,
        IBatchSerialRepository batchSerialRepository,
        IMainSerialRepository mainSerialRepository)
        : IRequestHandler<UpdateBatchSerialCommand, CustomResultResponse>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IBatchSerialRepository _batchSerialRepository = batchSerialRepository;
        private readonly IMainSerialRepository _mainSerialRepository = mainSerialRepository;
        private readonly ILogger<UpdateBatchSerialCommandHandler> _logger = logger; // Add logger

        /// <summary>
        /// Handles the update process for a Batch Serial using the specified ID.
        /// Performs validation, ensures the entity exists, maps the updated values, and saves changes to the database.
        /// </summary>
        /// <param name="request">The UpdateBatchSerialCommand containing the update details.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A CustomResultResponse indicating the success or failure of the update process.</returns>
        /// <exception cref="BadRequestException">Thrown when validation fails.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when the entity with the specified ID is not found.</exception>
        /// <exception cref="Exception">Thrown for any unexpected errors during the update process.</exception>
        public async Task<CustomResultResponse> Handle(UpdateBatchSerialCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check existing Batch ContractNo Details                
                var batchSerial = await _batchSerialRepository.GetBatchSerialsById(request.Id);
                if (batchSerial == null)
                {
                    _logger.LogWarning("Batch Contract details with Id {Id} not found.", request.Id);
                    return new CustomResultResponse
                    {
                        IsSuccess = false,
                        Message = $"Batch Contract with Id {request.Id} was not found."
                    };
                }

                bool isContractNoUnchanged = batchSerial.ContractNo == request.ContractNo;
                if (isContractNoUnchanged)
                {

                    var validationForExistingRecord = await new UpdateBatchSerialCommandValidatorForExistingContract(_batchSerialRepository)
                      .ValidateAsync(request, cancellationToken);

                    if (validationForExistingRecord.Errors.Count != 0)
                    {
                        _logger.LogWarning("Validation failed for UpdateBatchSerialCommand: {Errors}", validationForExistingRecord.Errors);
                        throw new BadRequestException("Validation failed", validationForExistingRecord);
                    }

                    _mapper.Map(request, batchSerial);

                    await using var transaction = await _batchSerialRepository.BeginTransactionAsync();

                    try
                    {
                        await _batchSerialRepository.UpdateAsync(batchSerial);
                        await _mainSerialRepository.UpdateContractNoAsync(batchSerial.ContractNo, request.ContractNo);
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Transaction failed. Rolling back changes.");
                        await transaction.RollbackAsync();
                        throw;
                    }

                    _logger.LogInformation("Batch and MainSerials successfully updated for ContractNo: {ContractNo}", request.ContractNo);

                    return new CustomResultResponse
                    {
                        IsSuccess = true,
                        Message = "Batch Contract details successfully updated.",
                        Id = request.Id.ToString()
                    };
                }

                // Validate data
                var validationResult = await new UpdateBatchSerialCommandValidator(_batchSerialRepository)
                        .ValidateAsync(request, cancellationToken);

                if (validationResult.Errors.Count != 0)
                {
                    _logger.LogWarning("Validation failed for UpdateBatchSerialCommand: {Errors}", validationResult.Errors);
                    throw new BadRequestException("Validation failed", validationResult);
                }

                // Get the Old Contract# to update MainSerials table
                string oldContractNo = batchSerial.ContractNo;

                // Map the updates to the existing entity
                _mapper.Map(request, batchSerial);

                //Use transaction for BatchSerial and MainSerial updates
                await using var updateTransaction = await _batchSerialRepository.BeginTransactionAsync();

                try
                {
                    await _batchSerialRepository.UpdateAsync(batchSerial);
                    await _mainSerialRepository.UpdateContractNoAsync(oldContractNo, batchSerial.ContractNo);

                    await updateTransaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Transaction failed. Rolling back changes.");
                    await updateTransaction.RollbackAsync();
                    throw;
                }

                _logger.LogInformation("Batch and MainSerials successfully updated for ContractNo: {ContractNo}", request.ContractNo);

                return new CustomResultResponse
                {
                    IsSuccess = true,
                    Message = "Batch Contract details successfully updated.",
                    Id = batchSerial.Id.ToString()
                };

            }
            catch (BadRequestException ex)
            {
                _logger.LogError(ex, "Validation error occurred for ContractNo: {ContractNo}", request.ContractNo);

                return new CustomResultResponse
                {
                    IsSuccess = false,
                    Message = "Handler Validation failed.",
                    ValidationErrors = ex.ValidationErrors, // Include validation errors here
                    Id = null
                };
            }
            catch (Exception ex)
            {
                // Log and handle exceptions appropriately
                _logger.LogError(ex, "An error occurred while updating the details for Batch Contract: {Id}", request.Id);
                return new CustomResultResponse
                {
                    IsSuccess = false,
                    Message = $"An error occurred: {ex.Message}",
                    Id = null
                };
            }
        }
    }
}
