using FluentValidation;

namespace Services.Features.Financials.CostCenters.UseCases.Queries
{
    public class GetByIdCostCenterRequestRequestValidator
        : AbstractValidator<GetByIdCostCenterRequest>
    {
        public GetByIdCostCenterRequestRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
