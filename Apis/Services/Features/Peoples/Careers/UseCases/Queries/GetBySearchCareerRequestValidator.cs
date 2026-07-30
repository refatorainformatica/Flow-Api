using FluentValidation;

namespace Services.Features.Peoples.Careers.UseCases.Queries
{
    public class GetBySearchCareerRequestValidator : AbstractValidator<GetBySearchCareerRequest>
    {
        public GetBySearchCareerRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
