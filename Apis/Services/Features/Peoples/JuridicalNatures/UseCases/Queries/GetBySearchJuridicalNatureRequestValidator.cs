using FluentValidation;

namespace Services.Features.Peoples.JuridicalNatures.UseCases.Queries
{
    public class GetBySearchJuridicalNatureRequestValidator
        : AbstractValidator<GetBySearchJuridicalNatureRequest>
    {
        public GetBySearchJuridicalNatureRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
