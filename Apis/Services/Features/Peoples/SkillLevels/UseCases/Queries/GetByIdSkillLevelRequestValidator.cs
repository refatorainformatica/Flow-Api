using FluentValidation;

namespace Services.Features.Peoples.SkillLevels.UseCases.Queries
{
    public class GetByIdSkillLevelRequestValidator : AbstractValidator<GetByIdSkillLevelRequest>
    {
        public GetByIdSkillLevelRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
