using AutoMapper;
using Services.Features.Peoples.Sponsors.Models;
using Services.Features.Peoples.Sponsors.UseCases.Commands;
using Services.Features.Peoples.Sponsors.UseCases.Queries;
using Services.Features.Settings.DocumentTypes.Models;

namespace Services.Features.Peoples.Sponsors.Mappings
{
    public class SponsorMappingProfile : Profile
    {
        public SponsorMappingProfile()
        {
            CreateMap<Sponsor, CreateSponsorRequest>();
            CreateMap<Sponsor, EditSponsorRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<Sponsor, RemoveSponsorRequest>();
            CreateMap<Sponsor, GetByIdSponsorRequest>();
            CreateMap<Sponsor, GetSponsorRequest>();
            CreateMap<Sponsor, GetBySearchSponsorRequest>();
            CreateMap<SponsorDocument, SponsorRequest.SponsorDocumentRequest>();

            CreateMap<Sponsor, SponsorResponse>();
            CreateMap<SponsorDocument, SponsorResponse.SponsorDocumentResponse>();
            CreateMap<DocumentType, DocumentTypeResponse>();
        }
    }
}
