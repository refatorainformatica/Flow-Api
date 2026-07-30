using FluentValidation;

namespace Services.Features.Financials.ContractStates.UseCases.Commands
{
    public class RemoveContractStateRequestValidator : AbstractValidator<RemoveContractStateRequest>
    {
        public RemoveContractStateRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
