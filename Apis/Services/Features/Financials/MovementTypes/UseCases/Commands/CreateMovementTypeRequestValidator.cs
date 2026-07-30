using FluentValidation;

namespace Services.Features.Financials.MovementTypes.UseCases.Commands
{
    public class CreateMovementTypeRequestValidator : AbstractValidator<CreateMovementTypeRequest>
    {
        public CreateMovementTypeRequestValidator()
        {
            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
