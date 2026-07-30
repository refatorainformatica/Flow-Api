using FluentValidation;

namespace Services.Features.Financials.CashFlows.UseCases.Commands
{
    public class CreateCashFlowRequestValidator : AbstractValidator<CreateCashFlowRequest>
    {
        public CreateCashFlowRequestValidator()
        {
            RuleFor(p => p.YearExercise).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.MonthExercise).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.MovementTypeId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.CurrencyTypeId).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
