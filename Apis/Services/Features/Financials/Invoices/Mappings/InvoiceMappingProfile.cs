using AutoMapper;
using Services.Features.Financials.Invoices.Models;
using Services.Features.Financials.Invoices.UseCases.Commands;
using Services.Features.Financials.Invoices.UseCases.Queries;

namespace Services.Features.Financials.Invoices.Mappings
{
    public class InvoiceMappingProfile : Profile
    {
        public InvoiceMappingProfile()
        {
            CreateMap<Invoice, CreateInvoiceRequest>();
            CreateMap<Invoice, EditInvoiceRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<Invoice, RemoveInvoiceRequest>();
            CreateMap<Invoice, GetByIdInvoiceRequest>();
            CreateMap<Invoice, GetInvoiceRequest>();
            CreateMap<Invoice, GetBySearchInvoiceRequest>();

            CreateMap<Invoice, InvoiceResponse>();
        }
    }
}
