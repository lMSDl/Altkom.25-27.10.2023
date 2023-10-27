using AutoMapper;

namespace GrpcService.AutoMapper
{
    public class PersonMappingProfile : Profile
    {
        public PersonMappingProfile()
        {
            CreateMap<Models.Person, GrpcService.Protos.Person>();
            CreateMap<GrpcService.Protos.Person, Models.Person>();
        }
    }
}
