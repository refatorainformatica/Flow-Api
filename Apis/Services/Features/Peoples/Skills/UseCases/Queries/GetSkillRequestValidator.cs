using FluentValidation;

namespace Services.Features.Peoples.Skills.UseCases.Queries
{
    public class GetSkillRequestValidator : AbstractValidator<GetSkillRequest>
    {
        public GetSkillRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
