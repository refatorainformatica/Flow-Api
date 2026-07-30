using FluentValidation;

namespace Services.Features.Peoples.ProfessionalProfiles.UseCases.Queries
{
    public class GetByIdProfessionalProfileRequestValidator
        : AbstractValidator<GetByIdProfessionalProfileRequest>
    {
        public GetByIdProfessionalProfileRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
