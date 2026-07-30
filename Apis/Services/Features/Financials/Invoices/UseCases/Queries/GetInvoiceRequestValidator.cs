using FluentValidation;

namespace Services.Features.Financials.Invoices.UseCases.Queries
{
    public class GetInvoiceRequestValidator : AbstractValidator<GetInvoiceRequest>
    {
        public GetInvoiceRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
