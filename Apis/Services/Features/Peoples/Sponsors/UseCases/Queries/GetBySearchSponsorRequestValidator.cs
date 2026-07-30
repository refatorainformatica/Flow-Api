using FluentValidation;

namespace Services.Features.Peoples.Sponsors.UseCases.Queries
{
    public class GetBySearchSponsorRequestValidator : AbstractValidator<GetBySearchSponsorRequest>
    {
        public GetBySearchSponsorRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
