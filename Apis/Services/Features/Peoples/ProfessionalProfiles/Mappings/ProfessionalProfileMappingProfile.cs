using AutoMapper;
using Services.Features.Peoples.ProfessionalProfiles.Models;
using Services.Features.Peoples.ProfessionalProfiles.UseCases.Commands;
using Services.Features.Peoples.ProfessionalProfiles.UseCases.Queries;

namespace Services.Features.Peoples.ProfessionalProfiles.Mappings
{
    public class ProfessionalProfileMappingProfile : Profile
    {
        public ProfessionalProfileMappingProfile()
        {
            CreateMap<ProfessionalProfile, CreateProfessionalProfileRequest>();
            CreateMap<ProfessionalProfile, EditProfessionalProfileRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<ProfessionalProfile, RemoveProfessionalProfileRequest>();
            CreateMap<ProfessionalProfile, GetByIdProfessionalProfileRequest>();
            CreateMap<ProfessionalProfile, GetProfessionalProfileRequest>();
            CreateMap<ProfessionalProfile, GetBySearchProfessionalProfileRequest>();

            CreateMap<ProfessionalProfile, ProfessionalProfileResponse>();
        }
    }
}
