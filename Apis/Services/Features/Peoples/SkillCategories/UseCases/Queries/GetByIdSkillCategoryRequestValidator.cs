using FluentValidation;

namespace Services.Features.Peoples.SkillCategories.UseCases.Queries
{
    public class GetByIdSkillCategoryRequestValidator
        : AbstractValidator<GetByIdSkillCategoryRequest>
    {
        public GetByIdSkillCategoryRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
