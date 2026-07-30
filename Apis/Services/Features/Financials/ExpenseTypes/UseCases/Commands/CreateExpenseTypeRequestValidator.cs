using FluentValidation;

namespace Services.Features.Financials.ExpenseTypes.UseCases.Commands
{
    public class CreateExpenseTypeRequestValidator : AbstractValidator<CreateExpenseTypeRequest>
    {
        public CreateExpenseTypeRequestValidator()
        {
            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
