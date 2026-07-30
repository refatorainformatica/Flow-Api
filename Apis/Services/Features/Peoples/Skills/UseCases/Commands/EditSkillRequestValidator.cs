using FluentValidation;

namespace Services.Features.Peoples.Skills.UseCases.Commands
{
    public class EditSkillRequestValidator : AbstractValidator<EditSkillRequest>
    {
        public EditSkillRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.TalentId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.SkillTypeId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.SkillCategoryId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.SkillLevelId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.SkillLevelMaxId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.SkillStateId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.StartDate).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
