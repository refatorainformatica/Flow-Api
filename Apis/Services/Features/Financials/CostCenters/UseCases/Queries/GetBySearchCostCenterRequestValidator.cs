using FluentValidation;

namespace Services.Features.Financials.CostCenters.UseCases.Queries
{
    public class GetBySearchCostCenterRequestValidator
        : AbstractValidator<GetBySearchCostCenterRequest>
    {
        public GetBySearchCostCenterRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
