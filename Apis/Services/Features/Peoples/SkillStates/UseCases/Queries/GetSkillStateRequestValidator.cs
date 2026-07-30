using FluentValidation;

namespace Services.Features.Peoples.SkillStates.UseCases.Queries
{
    public class GetSkillStateRequestValidator : AbstractValidator<GetSkillStateRequest>
    {
        public GetSkillStateRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
