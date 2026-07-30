using FluentValidation;

namespace Services.Features.Financials.ContractTypes.UseCases.Commands
{
    public class RemoveContractTypeRequestValidator : AbstractValidator<RemoveContractTypeRequest>
    {
        public RemoveContractTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
