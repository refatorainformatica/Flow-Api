using FluentValidation;

namespace Services.Features.Peoples.SkillLevels.UseCases.Commands
{
    public class CreateSkillLevelRequestValidator : AbstractValidator<CreateSkillLevelRequest>
    {
        public CreateSkillLevelRequestValidator()
        {
            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
