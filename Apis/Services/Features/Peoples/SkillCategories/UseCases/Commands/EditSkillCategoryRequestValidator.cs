using FluentValidation;

namespace Services.Features.Peoples.SkillCategories.UseCases.Commands
{
    public class EditSkillCategoryRequestValidator : AbstractValidator<EditSkillCategoryRequest>
    {
        public EditSkillCategoryRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
