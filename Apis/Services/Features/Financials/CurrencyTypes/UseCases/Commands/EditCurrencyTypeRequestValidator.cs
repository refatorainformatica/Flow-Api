using FluentValidation;

namespace Services.Features.Financials.CurrencyTypes.UseCases.Commands
{
    public class EditCurrencyTypeRequestValidator : AbstractValidator<EditCurrencyTypeRequest>
    {
        public EditCurrencyTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
