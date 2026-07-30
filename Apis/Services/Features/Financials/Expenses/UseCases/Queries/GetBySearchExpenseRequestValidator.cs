using FluentValidation;

namespace Services.Features.Financials.Expenses.UseCases.Queries
{
    public class GetBySearchExpenseRequestValidator : AbstractValidator<GetBySearchExpenseRequest>
    {
        public GetBySearchExpenseRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
