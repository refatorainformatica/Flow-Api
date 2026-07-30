using FluentValidation;

namespace Services.Features.Financials.InvoiceTypes.UseCases.Commands
{
    public class EditInvoiceTypeRequestValidator : AbstractValidator<EditInvoiceTypeRequest>
    {
        public EditInvoiceTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
