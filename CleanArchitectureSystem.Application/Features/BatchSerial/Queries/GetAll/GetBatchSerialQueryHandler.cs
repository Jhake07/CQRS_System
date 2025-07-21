﻿using AutoMapper;
using CleanArchitectureSystem.Application.Contracts.Interface;
using CleanArchitectureSystem.Application.Contracts.Logging;
using CleanArchitectureSystem.Application.DTO;
using MediatR;

namespace CleanArchitectureSystem.Application.Features.BatchSerial.Queries.GetAll
{
    public class GetBatchSerialQueryHandler(IMapper mapper,
        IBatchSerialRepository batchSerialRepository,
        IAppLogger<GetBatchSerialQueryHandler> logger) :
        IRequestHandler<GetBatchSerialQuery, List<BatchSerialDto>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IBatchSerialRepository _batchSerialRepository = batchSerialRepository;
        private readonly IAppLogger<GetBatchSerialQueryHandler> _logger = logger;

        public async Task<List<BatchSerialDto>> Handle(GetBatchSerialQuery request, CancellationToken cancellationToken)
        {
            // Query the database
            var batch = await _batchSerialRepository.GetAsync();

            // Convert data object to DTO
            var data = _mapper.Map<List<BatchSerialDto>>(batch);

            // Return the list of DTO object
            _logger.LogInformation("Batch serial retrieve successfully.");
            return data;
        }
    }
}
