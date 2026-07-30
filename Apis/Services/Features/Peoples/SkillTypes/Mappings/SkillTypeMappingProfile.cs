using AutoMapper;
using Services.Features.Peoples.SkillTypes.Models;
using Services.Features.Peoples.SkillTypes.UseCases.Commands;
using Services.Features.Peoples.SkillTypes.UseCases.Queries;

namespace Services.Features.Peoples.SkillTypes.Mappings
{
    public class SkillTypeMappingProfile : Profile
    {
        public SkillTypeMappingProfile()
        {
            CreateMap<SkillType, CreateSkillTypeRequest>();
            CreateMap<SkillType, EditSkillTypeRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<SkillType, RemoveSkillTypeRequest>();
            CreateMap<SkillType, GetByIdSkillTypeRequest>();
            CreateMap<SkillType, GetSkillTypeRequest>();
            CreateMap<SkillType, GetBySearchSkillTypeRequest>();

            CreateMap<SkillType, SkillTypeResponse>();
        }
    }
}
