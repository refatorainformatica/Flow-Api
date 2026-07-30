using FluentValidation;

namespace Services.Features.Financials.PaymentStates.UseCases.Queries
{
    public class GetPaymentStateRequestValidator : AbstractValidator<GetPaymentStateRequest>
    {
        public GetPaymentStateRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
