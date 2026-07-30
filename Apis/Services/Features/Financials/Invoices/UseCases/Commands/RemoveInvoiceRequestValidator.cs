using FluentValidation;

namespace Services.Features.Financials.Invoices.UseCases.Commands
{
    public class RemoveInvoiceRequestValidator : AbstractValidator<RemoveInvoiceRequest>
    {
        public RemoveInvoiceRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
