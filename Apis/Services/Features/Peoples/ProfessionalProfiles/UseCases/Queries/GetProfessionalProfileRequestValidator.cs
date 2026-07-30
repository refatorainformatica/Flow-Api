using FluentValidation;

namespace Services.Features.Peoples.ProfessionalProfiles.UseCases.Queries
{
    public class GetProfessionalProfileRequestValidator
        : AbstractValidator<GetProfessionalProfileRequest>
    {
        public GetProfessionalProfileRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
