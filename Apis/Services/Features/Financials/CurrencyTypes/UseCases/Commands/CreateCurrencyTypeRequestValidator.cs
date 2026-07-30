using FluentValidation;

namespace Services.Features.Financials.CurrencyTypes.UseCases.Commands
{
    public class CreateCurrencyTypeRequestValidator : AbstractValidator<CreateCurrencyTypeRequest>
    {
        public CreateCurrencyTypeRequestValidator()
        {
            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
