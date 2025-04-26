using MediatR;

namespace CleanArchitectureSystem.Application.Features.BatchSerial.Queries.GetAll
{
    public record GetBatchSerialQuery : IRequest<List<BatchSerialDto>>
    {
    }
}
