using FluentValidation;

namespace Services.Features.Peoples.EducationLevels.UseCases.Queries
{
    public class GetEducationLevelRequestValidator : AbstractValidator<GetEducationLevelRequest>
    {
        public GetEducationLevelRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
