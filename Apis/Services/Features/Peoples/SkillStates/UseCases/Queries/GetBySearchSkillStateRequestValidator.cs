using FluentValidation;

namespace Services.Features.Peoples.SkillStates.UseCases.Queries
{
    public class GetBySearchSkillStateRequestValidator
        : AbstractValidator<GetBySearchSkillStateRequest>
    {
        public GetBySearchSkillStateRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
