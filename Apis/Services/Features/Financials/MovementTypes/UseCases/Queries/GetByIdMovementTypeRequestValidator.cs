using FluentValidation;

namespace Services.Features.Financials.MovementTypes.UseCases.Queries
{
    public class GetByIdMovementTypeRequestValidator : AbstractValidator<GetByIdMovementTypeRequest>
    {
        public GetByIdMovementTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
