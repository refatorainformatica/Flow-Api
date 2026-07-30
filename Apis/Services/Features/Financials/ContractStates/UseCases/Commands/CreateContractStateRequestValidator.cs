using FluentValidation;

namespace Services.Features.Financials.ContractStates.UseCases.Commands
{
    public class CreateContractStateRequestValidator : AbstractValidator<CreateContractStateRequest>
    {
        public CreateContractStateRequestValidator()
        {
            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
