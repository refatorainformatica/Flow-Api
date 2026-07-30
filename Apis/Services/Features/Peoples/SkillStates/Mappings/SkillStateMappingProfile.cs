using AutoMapper;
using Services.Features.Peoples.SkillStates.Models;
using Services.Features.Peoples.SkillStates.UseCases.Commands;
using Services.Features.Peoples.SkillStates.UseCases.Queries;

namespace Services.Features.Peoples.SkillStates.Mappings
{
    public class SkillStateMappingProfile : Profile
    {
        public SkillStateMappingProfile()
        {
            CreateMap<SkillState, CreateSkillStateRequest>();
            CreateMap<SkillState, EditSkillStateRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<SkillState, RemoveSkillStateRequest>();
            CreateMap<SkillState, GetByIdSkillStateRequest>();
            CreateMap<SkillState, GetSkillStateRequest>();
            CreateMap<SkillState, GetBySearchSkillStateRequest>();

            CreateMap<SkillState, SkillStateResponse>();
        }
    }
}
