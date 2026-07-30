using FluentValidation;

namespace Services.Features.Peoples.EducationLevels.UseCases.Queries
{
    public class GetBySearchEducationLevelRequestValidator
        : AbstractValidator<GetBySearchEducationLevelRequest>
    {
        public GetBySearchEducationLevelRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
