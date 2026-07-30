using AutoMapper;
using Services.Features.Financials.Contracts.Models;
using Services.Features.Financials.Contracts.UseCases.Commands;
using Services.Features.Financials.Contracts.UseCases.Queries;
using Services.Features.Settings.DocumentTypes.Models;

namespace Services.Features.Financials.Contracts.Mappings
{
    public class ContractMappingProfile : Profile
    {
        public ContractMappingProfile()
        {
            CreateMap<Contract, CreateContractRequest>();
            CreateMap<Contract, EditContractRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<Contract, RemoveContractRequest>();
            CreateMap<Contract, GetByIdContractRequest>();
            CreateMap<Contract, GetContractRequest>();
            CreateMap<Contract, GetBySearchContractRequest>();

            CreateMap<Contract, ContractResponse>();
            CreateMap<ContractDocument, ContractResponse.ContractDocumentResponse>();
            CreateMap<ContractSubscription, ContractResponse.ContractSubscriptionResponse>();
            CreateMap<DocumentType, DocumentTypeResponse>();
        }
    }
}
