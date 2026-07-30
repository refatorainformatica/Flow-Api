using FluentValidation;

namespace Services.Features.Peoples.JuridicalNatures.UseCases.Commands
{
    public class EditJuridicalNatureRequestValidator : AbstractValidator<EditJuridicalNatureRequest>
    {
        public EditJuridicalNatureRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
