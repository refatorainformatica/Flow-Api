using FluentValidation;

namespace Services.Features.Financials.Invoices.UseCases.Commands
{
    public class CreateInvoiceRequestValidator : AbstractValidator<CreateInvoiceRequest>
    {
        public CreateInvoiceRequestValidator()
        {
            RuleFor(p => p.SupplierId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.InvoiceTypeId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.InvoiceStateId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.File).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
