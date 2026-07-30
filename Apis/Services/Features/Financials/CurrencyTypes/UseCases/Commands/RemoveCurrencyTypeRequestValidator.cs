using FluentValidation;

namespace Services.Features.Financials.CurrencyTypes.UseCases.Commands
{
    public class RemoveCurrencyTypeRequestValidator : AbstractValidator<RemoveCurrencyTypeRequest>
    {
        public RemoveCurrencyTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
