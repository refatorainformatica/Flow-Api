using FluentValidation;

namespace Services.Features.Financials.Expenses.UseCases.Queries
{
    public class GetByIdExpenseRequestValidator : AbstractValidator<GetByIdExpenseRequest>
    {
        public GetByIdExpenseRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
