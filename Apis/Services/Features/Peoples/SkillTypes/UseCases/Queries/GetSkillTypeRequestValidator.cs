using FluentValidation;

namespace Services.Features.Peoples.SkillTypes.UseCases.Queries
{
    public class GetSkillTypeRequestValidator : AbstractValidator<GetSkillTypeRequest>
    {
        public GetSkillTypeRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
