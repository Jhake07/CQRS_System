using MediatR;

namespace CleanArchitectureSystem.Application.Features.BatchSerial.Queries.GetById
{
    public record GetBatchSerialByIdQuery(int Id) : IRequest<BatchSerialDto>
    {
    }
}
