using FluentValidation;

namespace Services.Features.Financials.PaymentStates.UseCases.Queries
{
    public class GetBySearchPaymentStateRequestValidator
        : AbstractValidator<GetBySearchPaymentStateRequest>
    {
        public GetBySearchPaymentStateRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
