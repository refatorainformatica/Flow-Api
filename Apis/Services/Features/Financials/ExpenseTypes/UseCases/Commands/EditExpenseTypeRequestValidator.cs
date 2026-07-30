using FluentValidation;

namespace Services.Features.Financials.ExpenseTypes.UseCases.Commands
{
    public class EditExpenseTypeRequestValidator : AbstractValidator<EditExpenseTypeRequest>
    {
        public EditExpenseTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
