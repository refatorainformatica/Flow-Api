using FluentValidation;

namespace Services.Features.Peoples.Careers.UseCases.Commands
{
    public class RemoveCareerRequestValidator : AbstractValidator<RemoveCareerRequest>
    {
        public RemoveCareerRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
