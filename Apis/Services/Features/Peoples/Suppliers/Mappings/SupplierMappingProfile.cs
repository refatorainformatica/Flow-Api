using AutoMapper;
using Services.Features.Peoples.Suppliers.Models;
using Services.Features.Peoples.Suppliers.UseCases.Commands;
using Services.Features.Peoples.Suppliers.UseCases.Queries;
using Services.Features.Settings.DocumentTypes.Models;

namespace Services.Features.Peoples.Suppliers.Mappings
{
    public class SupplierMappingProfile : Profile
    {
        public SupplierMappingProfile()
        {
            CreateMap<Supplier, CreateSupplierRequest>();
            CreateMap<Supplier, EditSupplierRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<Supplier, RemoveSupplierRequest>();
            CreateMap<Supplier, GetByIdSupplierRequest>();
            CreateMap<Supplier, GetSupplierRequest>();
            CreateMap<Supplier, GetBySearchSupplierRequest>();
            CreateMap<SupplierDocument, SupplierRequest.SupplierDocumentRequest>();

            CreateMap<Supplier, SupplierResponse>();
            CreateMap<SupplierDocument, SupplierResponse.SupplierDocumentResponse>();
            CreateMap<DocumentType, DocumentTypeResponse>();
        }
    }
}
