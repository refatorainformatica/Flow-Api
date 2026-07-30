using AutoMapper;
using Services.Features.Peoples.Sellers.Models;
using Services.Features.Peoples.Sellers.UseCases.Commands;
using Services.Features.Peoples.Sellers.UseCases.Queries;
using Services.Features.Settings.DocumentTypes.Models;

namespace Services.Features.Peoples.Sellers.Mappings
{
    public class SellerMappingProfile : Profile
    {
        public SellerMappingProfile()
        {
            CreateMap<Seller, CreateSellerRequest>();
            CreateMap<Seller, EditSellerRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<Seller, RemoveSellerRequest>();
            CreateMap<Seller, GetByIdSellerRequest>();
            CreateMap<Seller, GetSellerRequest>();
            CreateMap<Seller, GetBySearchSellerRequest>();
            CreateMap<SellerDocument, SellerRequest.SellerDocumentRequest>();

            CreateMap<Seller, SellerResponse>();
            CreateMap<SellerDocument, SellerResponse.SellerDocumentResponse>();
            CreateMap<DocumentType, DocumentTypeResponse>();
        }
    }
}
