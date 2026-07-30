using FluentValidation;

namespace Services.Features.Financials.InvoiceStates.UseCases.Queries
{
    public class GetBySearchInvoiceStateRequestValidator
        : AbstractValidator<GetBySearchInvoiceStateRequest>
    {
        public GetBySearchInvoiceStateRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
