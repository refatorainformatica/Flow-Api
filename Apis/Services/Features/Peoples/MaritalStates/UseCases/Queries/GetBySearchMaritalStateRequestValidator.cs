using FluentValidation;

namespace Services.Features.Peoples.MaritalStates.UseCases.Queries
{
    public class GetBySearchMaritalStateRequestValidator
        : AbstractValidator<GetBySearchMaritalStateRequest>
    {
        public GetBySearchMaritalStateRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
