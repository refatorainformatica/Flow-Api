using FluentValidation;

namespace Services.Features.Financials.PaymentStates.UseCases.Commands
{
    public class EditPaymentStateRequestValidator : AbstractValidator<EditPaymentStateRequest>
    {
        public EditPaymentStateRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
