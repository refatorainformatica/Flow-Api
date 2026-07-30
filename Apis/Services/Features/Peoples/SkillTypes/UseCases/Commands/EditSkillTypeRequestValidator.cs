using FluentValidation;

namespace Services.Features.Peoples.SkillTypes.UseCases.Commands
{
    public class EditSkillTypeRequestValidator : AbstractValidator<EditSkillTypeRequest>
    {
        public EditSkillTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Description).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
