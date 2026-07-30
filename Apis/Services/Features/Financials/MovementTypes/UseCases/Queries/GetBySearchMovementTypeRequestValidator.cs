using FluentValidation;

namespace Services.Features.Financials.MovementTypes.UseCases.Queries
{
    public class GetBySearchMovementTypeRequestValidator
        : AbstractValidator<GetBySearchMovementTypeRequest>
    {
        public GetBySearchMovementTypeRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
