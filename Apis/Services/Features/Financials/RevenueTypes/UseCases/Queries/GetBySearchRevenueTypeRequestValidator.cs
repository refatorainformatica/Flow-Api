using FluentValidation;

namespace Services.Features.Financials.RevenueTypes.UseCases.Queries
{
    public class GetBySearchRevenueTypeRequestValidator
        : AbstractValidator<GetBySearchRevenueTypeRequest>
    {
        public GetBySearchRevenueTypeRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
