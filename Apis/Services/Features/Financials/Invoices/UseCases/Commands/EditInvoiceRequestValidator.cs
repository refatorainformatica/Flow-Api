using FluentValidation;

namespace Services.Features.Financials.Invoices.UseCases.Commands
{
    public class EditInvoiceRequestValidator : AbstractValidator<EditInvoiceRequest>
    {
        public EditInvoiceRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.SupplierId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.InvoiceTypeId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.InvoiceStateId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.File).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
