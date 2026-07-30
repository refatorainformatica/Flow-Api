using FluentValidation;

namespace Services.Features.Financials.ContractStates.UseCases.Commands
{
    public class EditContractStateRequestValidator : AbstractValidator<EditContractStateRequest>
    {
        public EditContractStateRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
