using FluentValidation;

namespace Services.Features.Peoples.MaritalStates.UseCases.Commands
{
    public class RemoveMaritalStateRequestValidator : AbstractValidator<RemoveMaritalStateRequest>
    {
        public RemoveMaritalStateRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
