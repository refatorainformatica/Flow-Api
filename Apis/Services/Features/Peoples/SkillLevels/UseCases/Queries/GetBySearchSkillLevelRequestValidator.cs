using FluentValidation;

namespace Services.Features.Peoples.SkillLevels.UseCases.Queries
{
    public class GetBySearchSkillLevelRequestValidator
        : AbstractValidator<GetBySearchSkillLevelRequest>
    {
        public GetBySearchSkillLevelRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
