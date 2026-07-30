using FluentValidation;

namespace Services.Features.Financials.CostCenters.UseCases.Queries
{
    public class GetCostCenterRequestValidator : AbstractValidator<GetCostCenterRequest>
    {
        public GetCostCenterRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
