using FluentValidation;

namespace Services.Features.Financials.Banks.UseCases.Queries
{
    public class GetBySearchBankRequestValidator : AbstractValidator<GetBySearchBankRequest>
    {
        public GetBySearchBankRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
