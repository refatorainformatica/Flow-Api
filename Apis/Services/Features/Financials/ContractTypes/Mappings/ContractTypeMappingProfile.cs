using AutoMapper;
using Services.Features.Financials.ContractTypes.Models;
using Services.Features.Financials.ContractTypes.UseCases.Commands;
using Services.Features.Financials.ContractTypes.UseCases.Queries;

namespace Services.Features.Financials.ContractTypes.Mappings
{
    public class ContractTypeMappingProfile : Profile
    {
        public ContractTypeMappingProfile()
        {
            CreateMap<ContractType, CreateContractTypeRequest>();
            CreateMap<ContractType, EditContractTypeRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<ContractType, RemoveContractTypeRequest>();
            CreateMap<ContractType, GetByIdContractTypeRequest>();
            CreateMap<ContractType, GetContractTypeRequest>();
            CreateMap<ContractType, GetBySearchContractTypeRequest>();

            CreateMap<ContractType, ContractTypeResponse>();
        }
    }
}
