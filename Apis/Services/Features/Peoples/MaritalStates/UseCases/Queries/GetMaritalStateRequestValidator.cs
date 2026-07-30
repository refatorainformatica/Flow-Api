using FluentValidation;

namespace Services.Features.Peoples.MaritalStates.UseCases.Queries
{
    public class GetMaritalStateRequestValidator : AbstractValidator<GetMaritalStateRequest>
    {
        public GetMaritalStateRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
