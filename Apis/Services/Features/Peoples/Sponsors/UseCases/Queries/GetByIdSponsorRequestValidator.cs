using FluentValidation;

namespace Services.Features.Peoples.Sponsors.UseCases.Queries
{
    public class GetByIdSponsorRequestValidator : AbstractValidator<GetByIdSponsorRequest>
    {
        public GetByIdSponsorRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
