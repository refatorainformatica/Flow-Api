using AutoMapper;
using Services.Features.Financials.ExpenseTypes.Models;
using Services.Features.Financials.ExpenseTypes.UseCases.Commands;
using Services.Features.Financials.ExpenseTypes.UseCases.Queries;

namespace Services.Features.Financials.ExpenseTypes.Mappings
{
    public class ExpenseTypeMappingProfile : Profile
    {
        public ExpenseTypeMappingProfile()
        {
            CreateMap<ExpenseType, CreateExpenseTypeRequest>();
            CreateMap<ExpenseType, EditExpenseTypeRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<ExpenseType, RemoveExpenseTypeRequest>();
            CreateMap<ExpenseType, GetByIdExpenseTypeRequest>();
            CreateMap<ExpenseType, GetExpenseTypeRequest>();
            CreateMap<ExpenseType, GetBySearchExpenseTypeRequest>();

            CreateMap<ExpenseType, ExpenseTypeResponse>();
        }
    }
}
