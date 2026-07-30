using FluentValidation;

namespace Services.Features.Peoples.Skills.UseCases.Commands
{
    public class RemoveSkillRequestValidator : AbstractValidator<RemoveSkillRequest>
    {
        public RemoveSkillRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
