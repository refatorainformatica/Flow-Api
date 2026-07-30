using FluentValidation;

namespace Services.Features.Financials.MovementTypes.UseCases.Queries
{
    public class GetMovementTypeRequestValidator : AbstractValidator<GetMovementTypeRequest>
    {
        public GetMovementTypeRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
