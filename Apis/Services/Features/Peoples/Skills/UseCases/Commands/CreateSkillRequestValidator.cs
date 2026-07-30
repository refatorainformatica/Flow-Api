using FluentValidation;

namespace Services.Features.Peoples.Skills.UseCases.Commands
{
    public class CreateSkillRequestValidator : AbstractValidator<CreateSkillRequest>
    {
        public CreateSkillRequestValidator()
        {
            RuleFor(p => p.TalentId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.SkillTypeId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.SkillCategoryId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.SkillLevelId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.SkillLevelMaxId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.SkillStateId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.StartDate).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
