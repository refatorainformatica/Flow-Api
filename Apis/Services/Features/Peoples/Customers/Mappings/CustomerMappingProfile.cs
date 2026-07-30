using AutoMapper;
using Services.Features.Peoples.Customers.Models;
using Services.Features.Peoples.Customers.UseCases.Commands;
using Services.Features.Peoples.Customers.UseCases.Queries;
using Services.Features.Settings.DocumentTypes.Models;

namespace Services.Features.Peoples.Customers.Mappings
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<Customer, CreateCustomerRequest>();
            CreateMap<Customer, EditCustomerRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<Customer, RemoveCustomerRequest>();
            CreateMap<Customer, GetByIdCustomerRequest>();
            CreateMap<Customer, GetCustomerRequest>();
            CreateMap<Customer, GetBySearchCustomerRequest>();
            CreateMap<CustomerDocument, CustomerRequest.CustomerDocumentRequest>();

            CreateMap<Customer, CustomerResponse>();
            CreateMap<CustomerDocument, CustomerResponse.CustomerDocumentResponse>();
            CreateMap<DocumentType, DocumentTypeResponse>();
        }
    }
}
