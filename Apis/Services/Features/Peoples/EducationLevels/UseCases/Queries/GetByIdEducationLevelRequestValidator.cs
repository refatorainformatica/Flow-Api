using FluentValidation;

namespace Services.Features.Peoples.EducationLevels.UseCases.Queries
{
    public class GetByIdEducationLevelRequestValidator
        : AbstractValidator<GetByIdEducationLevelRequest>
    {
        public GetByIdEducationLevelRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
