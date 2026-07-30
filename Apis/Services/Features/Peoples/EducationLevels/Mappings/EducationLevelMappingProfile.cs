using AutoMapper;
using Services.Features.Peoples.EducationLevels.Models;
using Services.Features.Peoples.EducationLevels.UseCases.Commands;
using Services.Features.Peoples.EducationLevels.UseCases.Queries;

namespace Services.Features.Peoples.EducationLevels.Mappings
{
    public class EducationLevelMappingProfile : Profile
    {
        public EducationLevelMappingProfile()
        {
            CreateMap<EducationLevel, CreateEducationLevelRequest>();
            CreateMap<EducationLevel, EditEducationLevelRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<EducationLevel, RemoveEducationLevelRequest>();
            CreateMap<EducationLevel, GetByIdEducationLevelRequest>();
            CreateMap<EducationLevel, GetEducationLevelRequest>();
            CreateMap<EducationLevel, GetBySearchEducationLevelRequest>();

            CreateMap<EducationLevel, EducationLevelResponse>();
        }
    }
}
