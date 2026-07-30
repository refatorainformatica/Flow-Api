using FluentValidation;

namespace Services.Features.Peoples.SkillTypes.UseCases.Queries
{
    public class GetByIdSkillTypeRequestValidator : AbstractValidator<GetByIdSkillTypeRequest>
    {
        public GetByIdSkillTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
