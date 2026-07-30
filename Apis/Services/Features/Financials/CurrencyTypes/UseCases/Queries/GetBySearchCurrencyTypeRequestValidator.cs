using FluentValidation;

namespace Services.Features.Financials.CurrencyTypes.UseCases.Queries
{
    public class GetBySearchCurrencyTypeRequestValidator
        : AbstractValidator<GetBySearchCurrencyTypeRequest>
    {
        public GetBySearchCurrencyTypeRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
