using FluentValidation;

namespace Services.Features.Financials.MovementTypes.UseCases.Commands
{
    public class EditMovementTypeRequestValidator : AbstractValidator<EditMovementTypeRequest>
    {
        public EditMovementTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
