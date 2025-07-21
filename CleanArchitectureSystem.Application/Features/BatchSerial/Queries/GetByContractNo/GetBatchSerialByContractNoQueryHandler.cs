using AutoMapper;
using CleanArchitectureSystem.Application.Contracts.Interface;
using CleanArchitectureSystem.Application.DTO;
using CleanArchitectureSystem.Application.Exceptions;
using MediatR;

namespace CleanArchitectureSystem.Application.Features.BatchSerial.Queries.GetByContractNo
{
    public class GetBatchSerialByContractNoQueryHandler(IMapper mapper, IBatchSerialRepository batchSerialRepository) : IRequestHandler<GetBatchSerialByContractNoQuery, BatchSerialDto>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IBatchSerialRepository _batchSerialRepository = batchSerialRepository;

        public async Task<BatchSerialDto> Handle(GetBatchSerialByContractNoQuery request, CancellationToken cancellationToken)
        {
            // Query the database
            var batch = await _batchSerialRepository.GetBatchSerialsByContractNo(request.ContractNo) ?? throw new NotFoundException(nameof(BatchSerial), request.ContractNo);

            // Convert data objects to DTO
            var data = _mapper.Map<BatchSerialDto>(batch);

            // Return DTO object
            return data;
        }
    }
}
