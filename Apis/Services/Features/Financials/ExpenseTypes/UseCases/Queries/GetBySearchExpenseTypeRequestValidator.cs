using FluentValidation;

namespace Services.Features.Financials.ExpenseTypes.UseCases.Queries
{
    public class GetBySearchExpenseTypeRequestValidator
        : AbstractValidator<GetBySearchExpenseTypeRequest>
    {
        public GetBySearchExpenseTypeRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
