using AutoMapper;
using CleanArchitectureSystem.Application.Features.BatchSerial;
using CleanArchitectureSystem.Application.Features.BatchSerial.Commands.CreateBatchSerials;
using CleanArchitectureSystem.Application.Features.BatchSerial.Commands.DeleteBatchSerials;
using CleanArchitectureSystem.Application.Features.BatchSerial.Commands.UpdateBatchSerials;
using CleanArchitectureSystem.Domain;

namespace CleanArchitectureSystem.Application.MappingProfiles
{
    public class BatchSerialProfile : Profile
    {
        public BatchSerialProfile()
        {
            CreateMap<CreateBatchSerialCommand, BatchSerial>();
            CreateMap<UpdateBatchSerialCommand, BatchSerial>();
            CreateMap<DeleteBatchSerialsCommand, BatchSerial>();
            CreateMap<BatchSerialDto, BatchSerial>().ReverseMap();
        }
    }
}
