using FluentValidation;

namespace Services.Features.Peoples.MaritalStates.UseCases.Commands
{
    public class CreateMaritalStateRequestValidator : AbstractValidator<CreateMaritalStateRequest>
    {
        public CreateMaritalStateRequestValidator()
        {
            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
