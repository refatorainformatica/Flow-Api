using FluentValidation;

namespace Services.Features.Financials.PaymentStates.UseCases.Commands
{
    public class CreatePaymentStateRequestValidator : AbstractValidator<CreatePaymentStateRequest>
    {
        public CreatePaymentStateRequestValidator()
        {
            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
