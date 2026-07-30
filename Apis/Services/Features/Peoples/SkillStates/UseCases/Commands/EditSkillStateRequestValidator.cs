using FluentValidation;

namespace Services.Features.Peoples.SkillStates.UseCases.Commands
{
    public class EditSkillStateRequestValidator : AbstractValidator<EditSkillStateRequest>
    {
        public EditSkillStateRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Description).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
