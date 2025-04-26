using AutoMapper;
using CleanArchitectureSystem.Application.Features.BatchSerial.Commands.CreateBatchSerials;
using CleanArchitectureSystem.Domain;

namespace CleanArchitectureSystem.Application.MappingProfiles
{
    public class MainSerialProfile : Profile
    {
        public MainSerialProfile()
        {
            CreateMap<CreateBatchSerialCommand, MainSerial>().ReverseMap();
        }
    }
}
