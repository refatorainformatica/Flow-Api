using FluentValidation;

namespace Services.Features.Peoples.SkillCategories.UseCases.Commands
{
    public class CreateSkillCategoryRequestValidator : AbstractValidator<CreateSkillCategoryRequest>
    {
        public CreateSkillCategoryRequestValidator()
        {
            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
