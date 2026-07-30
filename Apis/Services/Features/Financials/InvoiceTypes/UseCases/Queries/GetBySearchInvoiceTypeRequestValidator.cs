using FluentValidation;

namespace Services.Features.Financials.InvoiceTypes.UseCases.Queries
{
    public class GetBySearchInvoiceTypeRequestValidator
        : AbstractValidator<GetBySearchInvoiceTypeRequest>
    {
        public GetBySearchInvoiceTypeRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
