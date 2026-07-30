using FluentValidation;

namespace Services.Features.Peoples.EducationLevels.UseCases.Commands
{
    public class EditEducationLevelRequestValidator : AbstractValidator<EditEducationLevelRequest>
    {
        public EditEducationLevelRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
