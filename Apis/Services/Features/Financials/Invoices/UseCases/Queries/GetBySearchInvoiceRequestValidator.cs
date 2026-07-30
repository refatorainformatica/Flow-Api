using FluentValidation;

namespace Services.Features.Financials.Invoices.UseCases.Queries
{
    public class GetBySearchInvoiceRequestValidator : AbstractValidator<GetBySearchInvoiceRequest>
    {
        public GetBySearchInvoiceRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
