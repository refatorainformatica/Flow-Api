using FluentValidation;

namespace Services.Features.Peoples.Careers.UseCases.Queries
{
    public class GetByIdCareerRequestValidator : AbstractValidator<GetByIdCareerRequest>
    {
        public GetByIdCareerRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
