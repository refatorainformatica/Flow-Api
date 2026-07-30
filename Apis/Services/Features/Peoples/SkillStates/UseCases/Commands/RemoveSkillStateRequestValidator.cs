using FluentValidation;

namespace Services.Features.Peoples.SkillStates.UseCases.Commands
{
    public class RemoveSkillStateRequestValidator : AbstractValidator<RemoveSkillStateRequest>
    {
        public RemoveSkillStateRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
