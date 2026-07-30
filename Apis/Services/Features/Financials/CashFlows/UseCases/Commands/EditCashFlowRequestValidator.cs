using FluentValidation;

namespace Services.Features.Financials.CashFlows.UseCases.Commands
{
    public class EditCashFlowRequestValidator : AbstractValidator<EditCashFlowRequest>
    {
        public EditCashFlowRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .NotNull()
                .WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .NotNull()
                .WithMessage("{PropertyName} is required");

            RuleFor(p => p.YearExercise).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.MonthExercise).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.MovementTypeId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.CurrencyTypeId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
