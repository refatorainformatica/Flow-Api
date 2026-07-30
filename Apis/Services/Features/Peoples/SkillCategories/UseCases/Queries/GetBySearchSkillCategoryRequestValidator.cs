using FluentValidation;

namespace Services.Features.Peoples.SkillCategories.UseCases.Queries
{
    public class GetBySearchSkillCategoryRequestValidator
        : AbstractValidator<GetBySearchSkillCategoryRequest>
    {
        public GetBySearchSkillCategoryRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
