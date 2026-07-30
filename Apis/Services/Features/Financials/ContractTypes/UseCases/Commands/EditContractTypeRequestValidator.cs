using FluentValidation;

namespace Services.Features.Financials.ContractTypes.UseCases.Commands
{
    public class EditContractTypeRequestValidator : AbstractValidator<EditContractTypeRequest>
    {
        public EditContractTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
