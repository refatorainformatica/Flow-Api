using FluentValidation;

namespace Services.Features.Peoples.SkillStates.UseCases.Commands
{
    public class CreateSkillStateRequestValidator : AbstractValidator<CreateSkillStateRequest>
    {
        public CreateSkillStateRequestValidator()
        {
            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
