using FluentValidation;

namespace Services.Features.Financials.InvoiceStates.UseCases.Queries
{
    public class GetInvoiceStateRequestValidator : AbstractValidator<GetInvoiceStateRequest>
    {
        public GetInvoiceStateRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
