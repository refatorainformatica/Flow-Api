using FluentValidation;

namespace Services.Features.Financials.InvoiceTypes.UseCases.Queries
{
    public class GetInvoiceTypeRequestValidator : AbstractValidator<GetInvoiceTypeRequest>
    {
        public GetInvoiceTypeRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
