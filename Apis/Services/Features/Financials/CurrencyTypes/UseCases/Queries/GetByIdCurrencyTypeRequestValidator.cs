using FluentValidation;

namespace Services.Features.Financials.CurrencyTypes.UseCases.Queries
{
    public class GetByIdCurrencyTypeRequestValidator : AbstractValidator<GetByIdCurrencyTypeRequest>
    {
        public GetByIdCurrencyTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
