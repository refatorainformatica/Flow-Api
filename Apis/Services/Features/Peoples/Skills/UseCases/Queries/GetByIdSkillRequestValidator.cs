using FluentValidation;

namespace Services.Features.Peoples.Skills.UseCases.Queries
{
    public class GetByIdSkillRequestValidator : AbstractValidator<GetByIdSkillRequest>
    {
        public GetByIdSkillRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
