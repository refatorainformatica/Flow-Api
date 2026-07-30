using FluentValidation;

namespace Services.Features.Financials.Revenues.UseCases.Commands
{
    public class EditRevenueRequestValidator : AbstractValidator<EditRevenueRequest>
    {
        public EditRevenueRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.InvoiceId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.DateOfIssue).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.DateOfDue).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.DateOfPayment).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.CostCenterId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.PaymentStateId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.RevenueTypeId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
