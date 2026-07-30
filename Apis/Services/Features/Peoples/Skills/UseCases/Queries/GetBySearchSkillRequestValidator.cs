using FluentValidation;

namespace Services.Features.Peoples.Skills.UseCases.Queries
{
    public class GetBySearchSkillRequestValidator : AbstractValidator<GetBySearchSkillRequest>
    {
        public GetBySearchSkillRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
