using FluentValidation;

namespace Services.Features.Peoples.SkillTypes.UseCases.Queries
{
    public class GetBySearchSkillTypeRequestValidator
        : AbstractValidator<GetBySearchSkillTypeRequest>
    {
        public GetBySearchSkillTypeRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
