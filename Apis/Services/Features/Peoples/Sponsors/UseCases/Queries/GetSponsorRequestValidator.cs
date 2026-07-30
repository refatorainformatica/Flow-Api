using FluentValidation;

namespace Services.Features.Peoples.Sponsors.UseCases.Queries
{
    public class GetSponsorRequestValidator : AbstractValidator<GetSponsorRequest>
    {
        public GetSponsorRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
