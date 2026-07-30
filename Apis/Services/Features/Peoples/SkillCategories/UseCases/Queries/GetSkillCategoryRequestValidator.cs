using FluentValidation;

namespace Services.Features.Peoples.SkillCategories.UseCases.Queries
{
    public class GetSkillCategoryRequestValidator : AbstractValidator<GetSkillCategoryRequest>
    {
        public GetSkillCategoryRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
