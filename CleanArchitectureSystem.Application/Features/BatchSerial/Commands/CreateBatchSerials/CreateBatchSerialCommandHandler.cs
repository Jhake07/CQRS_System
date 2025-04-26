using AutoMapper;
using CleanArchitectureSystem.Application.Contracts.Interface;
using CleanArchitectureSystem.Application.Exceptions;
using CleanArchitectureSystem.Application.Response;
using MediatR;
using Microsoft.Extensions.Logging; // Add the ILogger namespace

namespace CleanArchitectureSystem.Application.Features.BatchSerial.Commands.CreateBatchSerials
{
    public class CreateBatchSerialCommandHandler(
        IMapper mapper,
        IBatchSerialRepository batchSerialRepository,
        IMainSerialRepository mainSerialRepository,
        ILogger<CreateBatchSerialCommandHandler> logger)
        :
        IRequestHandler<CreateBatchSerialCommand, CustomResultResponse>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IBatchSerialRepository _batchSerialRepository = batchSerialRepository;
        private readonly IMainSerialRepository _mainSerialRepository = mainSerialRepository;
        private readonly ILogger<CreateBatchSerialCommandHandler> _logger = logger; // Add logger

        public async Task<CustomResultResponse> Handle(CreateBatchSerialCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate data
                var validator = new CreateBatchSerialCommandValidator(_batchSerialRepository);
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (validationResult.Errors.Count != 0)
                {
                    _logger.LogWarning("Validation failed for CreateBatchSerialCommand: {Errors}", validationResult.Errors);
                    throw new BadRequestException("Validation failed", validationResult);
                }

                // Map to domain model 
                var batchContract = _mapper.Map<Domain.BatchSerial>(request);

                // Save Batch Contract
                await _batchSerialRepository.CreateAsync(batchContract);
                _logger.LogInformation("Batch Contract saved successfully for ContractNo: {ContractNo}", request.ContractNo);

                // Generate and save main serials
                await CreateMainSerialsAsync(request, cancellationToken);
                _logger.LogInformation("Batch Serial created successfully for ContractNo: {ContractNo}", request.ContractNo);

                // Return a success message
                return new CustomResultResponse
                {
                    IsSuccess = true,
                    Message = "BatchSerial and MainSerials created successfully.",
                    Id = batchContract.Id.ToString() // Ensure the ID is converted to string if necessary
                };
            }
            catch (BadRequestException ex)
            {
                _logger.LogError(ex, "Validation error occurred for ContractNo: {ContractNo}", request.ContractNo);

                // Return structured validation errors
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
                _logger.LogError(ex, "An error occurred while processing CreateBatchSerialCommand for ContractNo: {ContractNo}", request.ContractNo);
                return new CustomResultResponse
                {
                    IsSuccess = false,
                    Message = $"An error occurred: {ex.Message}",
                    Id = null
                };
            }
        }

        public async Task CreateMainSerialsAsync(CreateBatchSerialCommand request, CancellationToken cancellationToken)
        {
            // Initialize the total quantity of MainSerials to be created and the starting serial number
            int qty = request.BatchQty;
            int startSerialNo = int.Parse(request.StartSNo);

            // Create a list to store MainSerials temporarily for batch processing
            var mainSerials = new List<Domain.MainSerial>();

            // Define constants for batch size and maximum retry attempts
            const int batchSize = 100;
            const int maxRetries = 3;

            // Iterate through the quantity and generate individual MainSerials
            for (int i = 0; i < qty; i++)
            {
                // Format the serial number to be zero-padded (e.g., 00001)
                var serialNo = string.Format("{0:D5}", startSerialNo + i);

                // Add a new MainSerial to the list with properties populated from the request
                mainSerials.Add(new Domain.MainSerial
                {
                    SerialNo = $"{request.SerialPrefix}{serialNo}",
                    BatchSerial_ContractNo = request.ContractNo,
                    ScanTo = "test" // Replace with actual logic
                });

                // Check if the batch size is reached or it's the last MainSerial
                if (mainSerials.Count == batchSize || i == qty - 1)
                {
                    bool success = false; // Flag for successful batch saving
                    int attempt = 0; // Initialize retry attempt counter

                    // Retry saving the batch until success or maximum attempts are reached
                    while (!success && attempt < maxRetries)
                    {
                        try
                        {
                            attempt++; // Increment attempt count
                            _logger.LogInformation("Attempting to save batch of {BatchSize} MainSerials. Attempt {Attempt}/{MaxRetries}.", mainSerials.Count, attempt, maxRetries);

                            // Save the batch of MainSerials using the repository
                            await _mainSerialRepository.BulkCreateAsync(mainSerials, cancellationToken);
                            success = true; // Mark batch as successfully saved
                            _logger.LogInformation("Batch of {BatchSize} MainSerials saved successfully.", mainSerials.Count);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error saving batch of MainSerials. Attempt {Attempt}/{MaxRetries}.", attempt, maxRetries);

                            // If maximum retries are reached, save MainSerials individually
                            if (attempt == maxRetries)
                            {
                                _logger.LogCritical("Max retries reached. Fallback to saving individual MainSerials for ContractNo: {ContractNo}.", request.ContractNo);

                                foreach (var serial in mainSerials)
                                {
                                    try
                                    {
                                        // Save individual MainSerial
                                        await _mainSerialRepository.CreateAsync(serial);
                                        _logger.LogInformation("Successfully saved individual MainSerial: {SerialNo}.", serial.SerialNo);
                                    }
                                    catch (Exception innerEx)
                                    {
                                        // Log error for individual MainSerial save failure
                                        _logger.LogError(innerEx, "Failed to save individual MainSerial: {SerialNo}.", serial.SerialNo);
                                    }
                                }
                            }
                        }
                    }
                    // Clear the list after processing the batch to prepare for the next batch
                    mainSerials.Clear();
                }
            }
        }
    }
}
