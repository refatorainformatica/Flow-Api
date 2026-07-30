using FluentValidation;

namespace Services.Features.Financials.Revenues.UseCases.Queries
{
    public class GetByIdRevenueRequestValidator : AbstractValidator<GetByIdRevenueRequest>
    {
        public GetByIdRevenueRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
