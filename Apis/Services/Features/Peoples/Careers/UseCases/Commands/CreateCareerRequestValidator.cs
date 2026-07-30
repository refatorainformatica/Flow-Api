using FluentValidation;

namespace Services.Features.Peoples.Careers.UseCases.Commands
{
    public class CreateCareerRequestValidator : AbstractValidator<CreateCareerRequest>
    {
        public CreateCareerRequestValidator()
        {
            RuleFor(p => p.ExternalCode).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
