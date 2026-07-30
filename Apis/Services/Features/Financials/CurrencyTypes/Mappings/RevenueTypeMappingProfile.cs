using AutoMapper;
using Services.Features.Financials.RevenueTypes.Models;
using Services.Features.Financials.RevenueTypes.UseCases.Commands;
using Services.Features.Financials.RevenueTypes.UseCases.Queries;

namespace Services.Features.Financials.CurrencyTypes.Mappings
{
    public class RevenueTypeMappingProfile : Profile
    {
        public RevenueTypeMappingProfile()
        {
            CreateMap<RevenueType, CreateRevenueTypeRequest>();
            CreateMap<RevenueType, EditRevenueTypeRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<RevenueType, RemoveRevenueTypeRequest>();
            CreateMap<RevenueType, GetByIdRevenueTypeRequest>();
            CreateMap<RevenueType, GetRevenueTypeRequest>();
            CreateMap<RevenueType, GetBySearchRevenueTypeRequest>();

            CreateMap<RevenueType, RevenueTypeResponse>();
        }
    }
}
