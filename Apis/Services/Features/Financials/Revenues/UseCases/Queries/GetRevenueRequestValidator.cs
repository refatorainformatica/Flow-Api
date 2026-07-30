using FluentValidation;

namespace Services.Features.Financials.Revenues.UseCases.Queries
{
    public class GetRevenueRequestValidator : AbstractValidator<GetRevenueRequest>
    {
        public GetRevenueRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
