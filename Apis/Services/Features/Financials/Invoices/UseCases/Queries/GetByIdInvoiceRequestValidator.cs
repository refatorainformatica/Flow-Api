using FluentValidation;

namespace Services.Features.Financials.Invoices.UseCases.Queries
{
    public class GetByIdInvoiceRequestValidator : AbstractValidator<GetByIdInvoiceRequest>
    {
        public GetByIdInvoiceRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
