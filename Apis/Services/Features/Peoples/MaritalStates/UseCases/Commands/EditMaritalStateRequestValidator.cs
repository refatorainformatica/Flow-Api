using FluentValidation;

namespace Services.Features.Peoples.MaritalStates.UseCases.Commands
{
    public class EditMaritalStateRequestValidator : AbstractValidator<EditMaritalStateRequest>
    {
        public EditMaritalStateRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
