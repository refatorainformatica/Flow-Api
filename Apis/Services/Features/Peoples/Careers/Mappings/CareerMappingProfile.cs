using AutoMapper;
using Services.Features.Peoples.Careers.Models;
using Services.Features.Peoples.Careers.UseCases.Commands;
using Services.Features.Peoples.Careers.UseCases.Queries;

namespace Services.Features.Peoples.Careers.Mappings
{
    public class CareerMappingProfile : Profile
    {
        public CareerMappingProfile()
        {
            CreateMap<Career, CreateCareerRequest>();
            CreateMap<Career, EditCareerRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<Career, RemoveCareerRequest>();
            CreateMap<Career, GetByIdCareerRequest>();
            CreateMap<Career, GetCareerRequest>();
            CreateMap<Career, GetBySearchCareerRequest>();

            CreateMap<Career, CareerResponse>();
        }
    }
}
