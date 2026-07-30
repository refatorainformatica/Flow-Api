using FluentValidation;

namespace Services.Features.Financials.Expenses.UseCases.Queries
{
    public class GetExpenseRequestValidator : AbstractValidator<GetExpenseRequest>
    {
        public GetExpenseRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
