using FluentValidation;

namespace Services.Features.Peoples.MaritalStates.UseCases.Queries
{
    public class GetByIdMaritalStateRequestValidator : AbstractValidator<GetByIdMaritalStateRequest>
    {
        public GetByIdMaritalStateRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
