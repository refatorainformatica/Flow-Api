using FluentValidation;

namespace Services.Features.Financials.Contracts.UseCases.Commands
{
    public class RemoveContractRequestValidator : AbstractValidator<RemoveContractRequest>
    {
        public RemoveContractRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
