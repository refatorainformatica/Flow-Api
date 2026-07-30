using FluentValidation;

namespace Services.Features.Financials.RevenueTypes.UseCases.Queries
{
    public class GetByIdRevenueTypeRequestValidator : AbstractValidator<GetByIdRevenueTypeRequest>
    {
        public GetByIdRevenueTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
