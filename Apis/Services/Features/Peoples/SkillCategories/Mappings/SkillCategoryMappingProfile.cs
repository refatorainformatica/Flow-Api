using AutoMapper;
using Services.Features.Peoples.SkillCategories.Models;
using Services.Features.Peoples.SkillCategories.UseCases.Commands;
using Services.Features.Peoples.SkillCategories.UseCases.Queries;

namespace Services.Features.Peoples.SkillCategories.Mappings
{
    public class SkillCategoryMappingProfile : Profile
    {
        public SkillCategoryMappingProfile()
        {
            CreateMap<SkillCategory, CreateSkillCategoryRequest>();
            CreateMap<SkillCategory, EditSkillCategoryRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<SkillCategory, RemoveSkillCategoryRequest>();
            CreateMap<SkillCategory, GetByIdSkillCategoryRequest>();
            CreateMap<SkillCategory, GetSkillCategoryRequest>();
            CreateMap<SkillCategory, GetBySearchSkillCategoryRequest>();

            CreateMap<SkillCategory, SkillCategoryResponse>();
        }
    }
}
