using AutoMapper;
using Services.Features.Peoples.SkillLevels.Models;
using Services.Features.Peoples.SkillLevels.UseCases.Commands;
using Services.Features.Peoples.SkillLevels.UseCases.Queries;

namespace Services.Features.Peoples.SkillLevels.Mappings
{
    public class SkillLevelMappingProfile : Profile
    {
        public SkillLevelMappingProfile()
        {
            CreateMap<SkillLevel, CreateSkillLevelRequest>();
            CreateMap<SkillLevel, EditSkillLevelRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<SkillLevel, RemoveSkillLevelRequest>();
            CreateMap<SkillLevel, GetByIdSkillLevelRequest>();
            CreateMap<SkillLevel, GetSkillLevelRequest>();
            CreateMap<SkillLevel, GetBySearchSkillLevelRequest>();

            CreateMap<SkillLevel, SkillLevelResponse>();
        }
    }
}
