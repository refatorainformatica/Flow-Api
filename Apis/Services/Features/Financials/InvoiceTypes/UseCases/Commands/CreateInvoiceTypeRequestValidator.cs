using FluentValidation;

namespace Services.Features.Financials.InvoiceTypes.UseCases.Commands
{
    public class CreateInvoiceTypeRequestValidator : AbstractValidator<CreateInvoiceTypeRequest>
    {
        public CreateInvoiceTypeRequestValidator()
        {
            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
