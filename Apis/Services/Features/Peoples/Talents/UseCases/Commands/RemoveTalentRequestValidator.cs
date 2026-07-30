using FluentValidation;

namespace Services.Features.Peoples.Talents.UseCases.Commands
{
    public class RemoveTalentRequestValidator : AbstractValidator<RemoveTalentRequest>
    {
        public RemoveTalentRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
