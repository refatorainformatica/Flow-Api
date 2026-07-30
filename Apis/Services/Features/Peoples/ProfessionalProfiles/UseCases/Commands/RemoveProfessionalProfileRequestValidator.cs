using FluentValidation;

namespace Services.Features.Peoples.ProfessionalProfiles.UseCases.Commands
{
    public class RemoveProfessionalProfileRequestValidator
        : AbstractValidator<RemoveProfessionalProfileRequest>
    {
        public RemoveProfessionalProfileRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
