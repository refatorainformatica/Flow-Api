using AutoMapper;
using Services.Features.Financials.Banks.Models;
using Services.Features.Financials.Banks.UseCases.Commands;
using Services.Features.Financials.Banks.UseCases.Queries;

namespace Services.Features.Financials.Banks.Mappings
{
    public class BankMappingProfile : Profile
    {
        public BankMappingProfile()
        {
            CreateMap<Bank, CreateBankRequest>();
            CreateMap<Bank, EditBankRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<Bank, RemoveBankRequest>();
            CreateMap<Bank, GetByIdBankRequest>();
            CreateMap<Bank, GetBankRequest>();
            CreateMap<Bank, GetBySearchBankRequest>();

            CreateMap<Bank, BankResponse>();
        }
    }
}
