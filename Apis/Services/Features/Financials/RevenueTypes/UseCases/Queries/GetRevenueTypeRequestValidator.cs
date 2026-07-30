using FluentValidation;

namespace Services.Features.Financials.RevenueTypes.UseCases.Queries
{
    public class GetRevenueTypeRequestValidator : AbstractValidator<GetRevenueTypeRequest>
    {
        public GetRevenueTypeRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
