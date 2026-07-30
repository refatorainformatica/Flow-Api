using FluentValidation;

namespace Services.Features.Financials.InvoiceStates.UseCases.Commands
{
    public class CreateInvoiceStateRequestValidator : AbstractValidator<CreateInvoiceStateRequest>
    {
        public CreateInvoiceStateRequestValidator()
        {
            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
