using FluentValidation;

namespace Services.Features.Financials.Revenues.UseCases.Commands
{
    public class CreateRevenueRequestValidator : AbstractValidator<CreateRevenueRequest>
    {
        public CreateRevenueRequestValidator()
        {
            RuleFor(p => p.InvoiceId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.DateOfIssue).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.DateOfDue).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.DateOfPayment).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.CostCenterId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.PaymentStateId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.RevenueTypeId).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
