using MediatR;

namespace CleanArchitectureSystem.Application.Features.BatchSerial.Queries.GetByContractNo
{
    public record GetBatchSerialByContractNoQuery(string ContractNo) : IRequest<BatchSerialDto>
    {
    }
}
