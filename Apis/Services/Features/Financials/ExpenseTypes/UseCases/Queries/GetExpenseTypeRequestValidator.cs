using FluentValidation;

namespace Services.Features.Financials.ExpenseTypes.UseCases.Queries
{
    public class GetExpenseTypeRequestValidator : AbstractValidator<GetExpenseTypeRequest>
    {
        public GetExpenseTypeRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
