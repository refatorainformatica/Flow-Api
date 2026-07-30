using FluentValidation;

namespace Services.Features.Financials.Banks.UseCases.Commands
{
    public class CreateBankRequestValidator : AbstractValidator<CreateBankRequest>
    {
        public CreateBankRequestValidator()
        {
            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
