using AutoMapper;
using Services.Features.Financials.CostCenters.Models;
using Services.Features.Financials.CostCenters.UseCases.Commands;
using Services.Features.Financials.CostCenters.UseCases.Queries;

namespace Services.Features.Financials.CostCenters.Mappings
{
    public class CostCenterMappingProfile : Profile
    {
        public CostCenterMappingProfile()
        {
            CreateMap<CostCenter, CreateCostCenterRequest>();
            CreateMap<CostCenter, EditCostCenterRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<CostCenter, RemoveCostCenterRequest>();
            CreateMap<CostCenter, GetByIdCostCenterRequest>();
            CreateMap<CostCenter, GetCostCenterRequest>();
            CreateMap<CostCenter, GetBySearchCostCenterRequest>();

            CreateMap<CostCenter, CostCenterResponse>();
        }
    }
}
