using FluentValidation;

namespace Services.Features.Peoples.JuridicalNatures.UseCases.Queries
{
    public class GetByIdJuridicalNatureRequestValidator
        : AbstractValidator<GetByIdJuridicalNatureRequest>
    {
        public GetByIdJuridicalNatureRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
