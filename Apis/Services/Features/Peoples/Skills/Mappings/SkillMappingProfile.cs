using AutoMapper;
using Services.Features.Peoples.Skills.Models;
using Services.Features.Peoples.Skills.UseCases.Commands;
using Services.Features.Peoples.Skills.UseCases.Queries;

namespace Services.Features.Peoples.Skills.Mappings
{
    public class SkillMappingProfile : Profile
    {
        public SkillMappingProfile()
        {
            CreateMap<Skill, CreateSkillRequest>();
            CreateMap<Skill, EditSkillRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<Skill, RemoveSkillRequest>();
            CreateMap<Skill, GetByIdSkillRequest>();
            CreateMap<Skill, GetSkillRequest>();
            CreateMap<Skill, GetBySearchSkillRequest>();

            CreateMap<Skill, SkillResponse>();
        }
    }
}
