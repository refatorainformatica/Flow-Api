using AutoMapper;
using Services.Features.Financials.InvoiceStates.Models;
using Services.Features.Financials.InvoiceStates.UseCases.Commands;
using Services.Features.Financials.InvoiceStates.UseCases.Queries;

namespace Services.Features.Financials.InvoiceStates.Mappings
{
    public class InvoiceStateMappingProfile : Profile
    {
        public InvoiceStateMappingProfile()
        {
            CreateMap<InvoiceState, CreateInvoiceStateRequest>();
            CreateMap<InvoiceState, EditInvoiceStateRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<InvoiceState, RemoveInvoiceStateRequest>();
            CreateMap<InvoiceState, GetByIdInvoiceStateRequest>();
            CreateMap<InvoiceState, GetInvoiceStateRequest>();
            CreateMap<InvoiceState, GetBySearchInvoiceStateRequest>();

            CreateMap<InvoiceState, InvoiceStateResponse>();
        }
    }
}
