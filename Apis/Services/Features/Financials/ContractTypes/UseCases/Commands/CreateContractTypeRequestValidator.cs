using FluentValidation;

namespace Services.Features.Financials.ContractTypes.UseCases.Commands
{
    public class CreateContractTypeRequestValidator : AbstractValidator<CreateContractTypeRequest>
    {
        public CreateContractTypeRequestValidator()
        {
            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
