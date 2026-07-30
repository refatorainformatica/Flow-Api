using FluentValidation;

namespace Services.Features.Peoples.SkillCategories.UseCases.Commands
{
    public class RemoveSkillCategoryRequestValidator : AbstractValidator<RemoveSkillCategoryRequest>
    {
        public RemoveSkillCategoryRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
