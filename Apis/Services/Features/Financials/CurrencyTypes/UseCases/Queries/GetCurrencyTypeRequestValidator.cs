using FluentValidation;

namespace Services.Features.Financials.CurrencyTypes.UseCases.Queries
{
    public class GetCurrencyTypeRequestValidator : AbstractValidator<GetCurrencyTypeRequest>
    {
        public GetCurrencyTypeRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
