using FluentValidation;

namespace Services.Features.Peoples.JuridicalNatures.UseCases.Commands
{
    public class RemoveJuridicalNatureRequestValidator
        : AbstractValidator<RemoveJuridicalNatureRequest>
    {
        public RemoveJuridicalNatureRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
