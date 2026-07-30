using FluentValidation;

namespace Services.Features.Financials.Revenues.UseCases.Queries
{
    public class GetBySearchRevenueRequestValidator : AbstractValidator<GetBySearchRevenueRequest>
    {
        public GetBySearchRevenueRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
