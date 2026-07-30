using FluentValidation;

namespace Services.Features.Peoples.SkillTypes.UseCases.Commands
{
    public class CreateSkillTypeRequestValidator : AbstractValidator<CreateSkillTypeRequest>
    {
        public CreateSkillTypeRequestValidator()
        {
            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
