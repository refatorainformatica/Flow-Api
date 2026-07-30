using FluentValidation;

namespace Services.Features.Peoples.JuridicalNatures.UseCases.Commands
{
    public class CreateJuridicalNatureRequestValidator
        : AbstractValidator<CreateJuridicalNatureRequest>
    {
        public CreateJuridicalNatureRequestValidator()
        {
            RuleFor(p => p.ExternalCode).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
