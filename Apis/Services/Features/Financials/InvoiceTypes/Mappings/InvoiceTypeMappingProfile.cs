using AutoMapper;
using Services.Features.Financials.InvoiceTypes.Models;
using Services.Features.Financials.InvoiceTypes.UseCases.Commands;
using Services.Features.Financials.InvoiceTypes.UseCases.Queries;

namespace Services.Features.Financials.InvoiceTypes.Mappings
{
    public class InvoiceTypeMappingProfile : Profile
    {
        public InvoiceTypeMappingProfile()
        {
            CreateMap<InvoiceType, CreateInvoiceTypeRequest>();
            CreateMap<InvoiceType, EditInvoiceTypeRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<InvoiceType, RemoveInvoiceTypeRequest>();
            CreateMap<InvoiceType, GetByIdInvoiceTypeRequest>();
            CreateMap<InvoiceType, GetInvoiceTypeRequest>();
            CreateMap<InvoiceType, GetBySearchInvoiceTypeRequest>();

            CreateMap<InvoiceType, InvoiceTypeResponse>();
        }
    }
}
