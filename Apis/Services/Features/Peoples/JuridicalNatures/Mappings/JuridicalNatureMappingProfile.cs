using AutoMapper;
using Services.Features.Peoples.JuridicalNatures.Models;
using Services.Features.Peoples.JuridicalNatures.UseCases.Commands;
using Services.Features.Peoples.JuridicalNatures.UseCases.Queries;

namespace Services.Features.Peoples.JuridicalNatures.Mappings
{
    public class JuridicalNatureMappingProfile : Profile
    {
        public JuridicalNatureMappingProfile()
        {
            CreateMap<JuridicalNature, CreateJuridicalNatureRequest>();
            CreateMap<JuridicalNature, EditJuridicalNatureRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<JuridicalNature, RemoveJuridicalNatureRequest>();
            CreateMap<JuridicalNature, GetByIdJuridicalNatureRequest>();
            CreateMap<JuridicalNature, GetJuridicalNatureRequest>();
            CreateMap<JuridicalNature, GetBySearchJuridicalNatureRequest>();

            CreateMap<JuridicalNature, JuridicalNatureResponse>();
        }
    }
}
