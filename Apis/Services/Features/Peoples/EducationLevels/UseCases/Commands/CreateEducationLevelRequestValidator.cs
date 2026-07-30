using FluentValidation;

namespace Services.Features.Peoples.EducationLevels.UseCases.Commands
{
    public class CreateEducationLevelRequestValidator
        : AbstractValidator<CreateEducationLevelRequest>
    {
        public CreateEducationLevelRequestValidator()
        {
            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
