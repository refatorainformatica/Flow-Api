using FluentValidation;

namespace Services.Features.Financials.Expenses.UseCases.Commands
{
    public class RemoveExpenseRequestValidator : AbstractValidator<RemoveExpenseRequest>
    {
        public RemoveExpenseRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
