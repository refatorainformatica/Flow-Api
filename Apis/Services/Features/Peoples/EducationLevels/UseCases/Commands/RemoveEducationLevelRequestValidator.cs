using FluentValidation;

namespace Services.Features.Peoples.EducationLevels.UseCases.Commands
{
    public class RemoveEducationLevelRequestValidator
        : AbstractValidator<RemoveEducationLevelRequest>
    {
        public RemoveEducationLevelRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
