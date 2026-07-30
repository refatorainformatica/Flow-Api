using AutoMapper;
using Services.Features.Peoples.MaritalStates.Models;
using Services.Features.Peoples.MaritalStates.UseCases.Commands;
using Services.Features.Peoples.MaritalStates.UseCases.Queries;

namespace Services.Features.Peoples.MaritalStates.Mappings
{
    public class MaritalStateMappingProfile : Profile
    {
        public MaritalStateMappingProfile()
        {
            CreateMap<MaritalState, CreateMaritalStateRequest>();
            CreateMap<MaritalState, EditMaritalStateRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<MaritalState, RemoveMaritalStateRequest>();
            CreateMap<MaritalState, GetByIdMaritalStateRequest>();
            CreateMap<MaritalState, GetMaritalStateRequest>();
            CreateMap<MaritalState, GetBySearchMaritalStateRequest>();

            CreateMap<MaritalState, MaritalStateResponse>();
        }
    }
}
