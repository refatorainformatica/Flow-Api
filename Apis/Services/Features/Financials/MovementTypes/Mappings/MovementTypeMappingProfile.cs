using AutoMapper;
using Services.Features.Financials.MovementTypes.Models;
using Services.Features.Financials.MovementTypes.UseCases.Commands;
using Services.Features.Financials.MovementTypes.UseCases.Queries;

namespace Services.Features.Financials.MovementTypes.Mappings
{
    public class MovementTypeMappingProfile : Profile
    {
        public MovementTypeMappingProfile()
        {
            CreateMap<MovementType, CreateMovementTypeRequest>();
            CreateMap<MovementType, EditMovementTypeRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<MovementType, RemoveMovementTypeRequest>();
            CreateMap<MovementType, GetByIdMovementTypeRequest>();
            CreateMap<MovementType, GetMovementTypeRequest>();
            CreateMap<MovementType, GetBySearchMovementTypeRequest>();

            CreateMap<MovementType, MovementTypeResponse>();
        }
    }
}
