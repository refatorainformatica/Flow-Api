using FluentValidation;

namespace Services.Features.Financials.ExpenseTypes.UseCases.Queries
{
    public class GetByIdExpenseTypeRequestValidator : AbstractValidator<GetByIdExpenseTypeRequest>
    {
        public GetByIdExpenseTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
