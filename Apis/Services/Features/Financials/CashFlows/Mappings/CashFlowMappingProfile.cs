using AutoMapper;
using Services.Features.Financials.CashFlows.Models;
using Services.Features.Financials.CashFlows.UseCases.Commands;
using Services.Features.Financials.CashFlows.UseCases.Queries;

namespace Services.Features.Financials.CashFlows.Mappings
{
    public class CashFlowMappingProfile : Profile
    {
        public CashFlowMappingProfile()
        {
            CreateMap<CashFlow, CreateCashFlowRequest>();
            CreateMap<CashFlow, EditCashFlowRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<CashFlow, RemoveCashFlowRequest>();
            CreateMap<CashFlow, GetByIdCashFlowRequest>();
            CreateMap<CashFlow, GetCashFlowRequest>();
            CreateMap<CashFlow, GetBySearchCashFlowRequest>();

            CreateMap<CashFlow, CashFlowResponse>();
        }
    }
}
