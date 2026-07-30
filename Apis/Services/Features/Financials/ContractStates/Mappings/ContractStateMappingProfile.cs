using AutoMapper;
using Services.Features.Financials.ContractStates.Models;
using Services.Features.Financials.ContractStates.UseCases.Commands;
using Services.Features.Financials.ContractStates.UseCases.Queries;

namespace Services.Features.Financials.ContractStates.Mappings
{
    public class ContractStateMappingProfile : Profile
    {
        public ContractStateMappingProfile()
        {
            CreateMap<ContractState, CreateContractStateRequest>();
            CreateMap<ContractState, EditContractStateRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<ContractState, RemoveContractStateRequest>();
            CreateMap<ContractState, GetByIdContractStateRequest>();
            CreateMap<ContractState, GetContractStateRequest>();
            CreateMap<ContractState, GetBySearchContractStateRequest>();

            CreateMap<ContractState, ContractStateResponse>();
        }
    }
}
