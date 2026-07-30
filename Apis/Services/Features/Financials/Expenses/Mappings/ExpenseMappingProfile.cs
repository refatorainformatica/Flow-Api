using AutoMapper;
using Services.Features.Financials.Expenses.Models;
using Services.Features.Financials.Expenses.UseCases.Commands;
using Services.Features.Financials.Expenses.UseCases.Queries;

namespace Services.Features.Financials.Expenses.Mappings
{
    public class ExpenseMappingProfile : Profile
    {
        public ExpenseMappingProfile()
        {
            CreateMap<Expense, CreateExpenseRequest>();
            CreateMap<Expense, EditExpenseRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<Expense, RemoveExpenseRequest>();
            CreateMap<Expense, GetByIdExpenseRequest>();
            CreateMap<Expense, GetExpenseRequest>();
            CreateMap<Expense, GetBySearchExpenseRequest>();

            CreateMap<Expense, ExpenseResponse>();
        }
    }
}
