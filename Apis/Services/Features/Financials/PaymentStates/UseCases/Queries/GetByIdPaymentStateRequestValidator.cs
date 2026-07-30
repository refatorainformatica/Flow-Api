using FluentValidation;

namespace Services.Features.Financials.PaymentStates.UseCases.Queries
{
    public class GetByIdPaymentStateRequestValidator : AbstractValidator<GetByIdPaymentStateRequest>
    {
        public GetByIdPaymentStateRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
