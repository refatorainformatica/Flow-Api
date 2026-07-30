using FluentValidation;

namespace Services.Features.Financials.InvoiceTypes.UseCases.Queries
{
    public class GetByIdInvoiceTypeRequestValidator : AbstractValidator<GetByIdInvoiceTypeRequest>
    {
        public GetByIdInvoiceTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
