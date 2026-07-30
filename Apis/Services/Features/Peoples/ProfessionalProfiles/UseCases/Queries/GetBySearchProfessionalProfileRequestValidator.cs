using FluentValidation;

namespace Services.Features.Peoples.ProfessionalProfiles.UseCases.Queries
{
    public class GetBySearchProfessionalProfileRequestValidator
        : AbstractValidator<GetBySearchProfessionalProfileRequest>
    {
        public GetBySearchProfessionalProfileRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
