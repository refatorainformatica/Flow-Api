using FluentValidation;

namespace Services.Features.Financials.InvoiceStates.UseCases.Commands
{
    public class RemoveInvoiceStateRequestValidator : AbstractValidator<RemoveInvoiceStateRequest>
    {
        public RemoveInvoiceStateRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
