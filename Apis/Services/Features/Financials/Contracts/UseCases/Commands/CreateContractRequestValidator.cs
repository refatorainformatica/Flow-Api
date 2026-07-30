using FluentValidation;

namespace Services.Features.Financials.Contracts.UseCases.Commands
{
    public class CreateContractRequestValidator : AbstractValidator<CreateContractRequest>
    {
        public CreateContractRequestValidator()
        {
            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
