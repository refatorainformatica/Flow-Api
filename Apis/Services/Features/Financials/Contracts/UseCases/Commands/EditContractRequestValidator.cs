using FluentValidation;

namespace Services.Features.Financials.Contracts.UseCases.Commands
{
    public class EditContractRequestValidator : AbstractValidator<EditContractRequest>
    {
        public EditContractRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
