using FluentValidation;

namespace Services.Features.Peoples.SkillStates.UseCases.Queries
{
    public class GetByIdSkillStateRequestValidator : AbstractValidator<GetByIdSkillStateRequest>
    {
        public GetByIdSkillStateRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
