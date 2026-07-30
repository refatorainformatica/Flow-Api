using FluentValidation;

namespace Services.Features.Financials.InvoiceStates.UseCases.Queries
{
    public class GetByIdInvoiceStateRequestValidator : AbstractValidator<GetByIdInvoiceStateRequest>
    {
        public GetByIdInvoiceStateRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
