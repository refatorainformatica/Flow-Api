using FluentValidation;

namespace Services.Features.Peoples.SkillLevels.UseCases.Commands
{
    public class RemoveSkillLevelRequestValidator : AbstractValidator<RemoveSkillLevelRequest>
    {
        public RemoveSkillLevelRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
