using FluentValidation;

namespace Services.Features.Financials.MovementTypes.UseCases.Commands
{
    public class RemoveMovementTypeRequestValidator : AbstractValidator<RemoveMovementTypeRequest>
    {
        public RemoveMovementTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
