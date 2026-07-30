using FluentValidation;

namespace Services.Features.Financials.InvoiceTypes.UseCases.Commands
{
    public class RemoveInvoiceTypeRequestValidator : AbstractValidator<RemoveInvoiceTypeRequest>
    {
        public RemoveInvoiceTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
