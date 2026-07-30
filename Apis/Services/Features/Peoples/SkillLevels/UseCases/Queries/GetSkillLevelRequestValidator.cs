using FluentValidation;

namespace Services.Features.Peoples.SkillLevels.UseCases.Queries
{
    public class GetSkillLevelRequestValidator : AbstractValidator<GetSkillLevelRequest>
    {
        public GetSkillLevelRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
