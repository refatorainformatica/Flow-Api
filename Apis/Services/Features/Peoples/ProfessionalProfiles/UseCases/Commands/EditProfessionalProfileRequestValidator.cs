using FluentValidation;

namespace Services.Features.Peoples.ProfessionalProfiles.UseCases.Commands
{
    public class EditProfessionalProfileRequestValidator
        : AbstractValidator<EditProfessionalProfileRequest>
    {
        public EditProfessionalProfileRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
