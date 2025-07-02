using CleanArchitectureSystem.Application.Response;
using MediatR;

namespace CleanArchitectureSystem.Application.Features.BatchSerial.Commands.DeleteBatchSerials
{
    public class DeleteBatchSerialsCommand : IRequest<CustomResultResponse>
    {
        public required int Id { get; set; }
        public string? Status { get; set; } = "Cancelled"; // Default status for deletion
    }
}
