using FluentValidation;

namespace Services.Features.Financials.PaymentStates.UseCases.Commands
{
    public class RemovePaymentStateRequestValidator : AbstractValidator<RemovePaymentStateRequest>
    {
        public RemovePaymentStateRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
