using FluentValidation;

namespace Services.Features.Peoples.SkillLevels.UseCases.Commands
{
    public class EditSkillLevelRequestValidator : AbstractValidator<EditSkillLevelRequest>
    {
        public EditSkillLevelRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Description).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
