using FluentValidation;

namespace Services.Features.Peoples.ProfessionalProfiles.UseCases.Commands
{
    public class CreateProfessionalProfileRequestValidator
        : AbstractValidator<CreateProfessionalProfileRequest>
    {
        public CreateProfessionalProfileRequestValidator()
        {
            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
