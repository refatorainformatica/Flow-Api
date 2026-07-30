using AutoMapper;
using Services.Features.Financials.Revenues.Models;
using Services.Features.Financials.Revenues.UseCases.Commands;
using Services.Features.Financials.Revenues.UseCases.Queries;

namespace Services.Features.Financials.Revenues.Mappings
{
    public class RevenueMappingProfile : Profile
    {
        public RevenueMappingProfile()
        {
            CreateMap<Revenue, CreateRevenueRequest>();
            CreateMap<Revenue, EditRevenueRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<Revenue, RemoveRevenueRequest>();
            CreateMap<Revenue, GetByIdRevenueRequest>();
            CreateMap<Revenue, GetRevenueRequest>();
            CreateMap<Revenue, GetBySearchRevenueRequest>();

            CreateMap<Revenue, RevenueResponse>();
        }
    }
}
