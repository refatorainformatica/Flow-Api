using FluentValidation;

namespace Services.Features.Financials.CostCenters.UseCases.Commands
{
    public class CreateCostCenterRequestValidator : AbstractValidator<CreateCostCenterRequest>
    {
        public CreateCostCenterRequestValidator()
        {
            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
