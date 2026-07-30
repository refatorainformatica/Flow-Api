using FluentValidation;

namespace Services.Features.Peoples.Careers.UseCases.Queries
{
    public class GetCareerRequestValidator : AbstractValidator<GetCareerRequest>
    {
        public GetCareerRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
