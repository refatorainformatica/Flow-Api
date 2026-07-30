using FluentValidation;

namespace Services.Features.Peoples.JuridicalNatures.UseCases.Queries
{
    public class GetJuridicalNatureRequestValidator : AbstractValidator<GetJuridicalNatureRequest>
    {
        public GetJuridicalNatureRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
