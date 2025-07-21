using AutoMapper;
using CleanArchitectureSystem.Application.Contracts.Interface;
using CleanArchitectureSystem.Application.DTO;
using CleanArchitectureSystem.Application.Exceptions;
using MediatR;

namespace CleanArchitectureSystem.Application.Features.BatchSerial.Queries.GetById
{
    public class GetBatchSerialByIdQueryHandler(IMapper mapper, IBatchSerialRepository batchSerialRepository) : IRequestHandler<GetBatchSerialByIdQuery, BatchSerialDto>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IBatchSerialRepository _batchSerialRepository = batchSerialRepository;

        public async Task<BatchSerialDto> Handle(GetBatchSerialByIdQuery request, CancellationToken cancellationToken)
        {
            // Query the database
            var batch = await _batchSerialRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException(nameof(BatchSerial), request.Id);

            // Convert data objects to DTO
            var data = _mapper.Map<BatchSerialDto>(batch);

            // Return DTO object
            return data;
        }
    }
}
