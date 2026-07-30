using FluentValidation;

namespace Services.Features.Peoples.SkillTypes.UseCases.Commands
{
    public class RemoveSkillTypeRequestValidator : AbstractValidator<RemoveSkillTypeRequest>
    {
        public RemoveSkillTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
