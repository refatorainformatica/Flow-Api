using AutoMapper;
using Services.Features.Settings.DocumentTypes.Models;
using Services.Features.Settings.DocumentTypes.UseCases.Commands;
using Services.Features.Settings.DocumentTypes.UseCases.Queries;

namespace Services.Features.Settings.DocumentTypes.Mappings
{
    public class DocumentTypeMappingProfile : Profile
    {
        public DocumentTypeMappingProfile()
        {
            CreateMap<DocumentType, CreateDocumentTypeRequest>();
            CreateMap<DocumentType, EditDocumentTypeRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<DocumentType, RemoveDocumentTypeRequest>();
            CreateMap<DocumentType, GetByIdDocumentTypeRequest>();
            CreateMap<DocumentType, GetDocumentTypeRequest>();
            CreateMap<DocumentType, GetBySearchDocumentTypeRequest>();

            CreateMap<DocumentType, DocumentTypeResponse>();
        }
    }
}
