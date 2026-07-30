using FluentValidation;

namespace Services.Features.Financials.Expenses.UseCases.Commands
{
    public class EditExpenseRequestValidator : AbstractValidator<EditExpenseRequest>
    {
        public EditExpenseRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.InvoiceId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.DateOfIssue).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.DateOfDue).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.DateOfPayment).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.CostCenterId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.PaymentStateId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.ExpenseTypeId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
