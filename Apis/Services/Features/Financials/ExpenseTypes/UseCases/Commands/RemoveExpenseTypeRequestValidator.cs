using FluentValidation;

namespace Services.Features.Financials.ExpenseTypes.UseCases.Commands
{
    public class RemoveExpenseTypeRequestValidator : AbstractValidator<RemoveExpenseTypeRequest>
    {
        public RemoveExpenseTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
