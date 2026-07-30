using AutoMapper;
using Services.Features.Peoples.SkillCategories.Models;
using Services.Features.Peoples.SkillLevels.Models;
using Services.Features.Peoples.SkillStates.Models;
using Services.Features.Peoples.SkillTypes.Models;
using Services.Features.Peoples.Talents.Models;
using Services.Features.Peoples.Talents.UseCases.Commands;
using Services.Features.Peoples.Talents.UseCases.Queries;
using Services.Features.Settings.DocumentTypes.Models;

namespace Services.Features.Peoples.Talents.Mappings
{
    public class TalentMappingProfile : Profile
    {
        public TalentMappingProfile()
        {
            CreateMap<Talent, CreateTalentRequest>();
            CreateMap<Talent, EditTalentRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<Talent, RemoveTalentRequest>();
            CreateMap<Talent, GetByIdTalentRequest>();
            CreateMap<Talent, GetTalentRequest>();
            CreateMap<Talent, GetBySearchTalentRequest>();
            CreateMap<TalentDocument, TalentRequest.TalentDocumentRequest>();

            CreateMap<Talent, TalentResponse>();
            CreateMap<TalentDocument, TalentResponse.TalentDocumentResponse>();
            CreateMap<DocumentType, DocumentTypeResponse>();
            CreateMap<SkillType, SkillTypeResponse>();
            CreateMap<SkillCategory, SkillCategoryResponse>();
            CreateMap<SkillLevel, SkillLevelResponse>();
            CreateMap<SkillState, SkillStateResponse>();
        }
    }
}
