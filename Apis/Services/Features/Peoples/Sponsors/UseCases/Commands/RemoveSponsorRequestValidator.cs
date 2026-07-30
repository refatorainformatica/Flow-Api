using FluentValidation;

namespace Services.Features.Peoples.Sponsors.UseCases.Commands
{
    public class RemoveSponsorRequestValidator : AbstractValidator<RemoveSponsorRequest>
    {
        public RemoveSponsorRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
