using FluentValidation;

namespace Services.Features.Peoples.Talents.UseCases.Queries
{
    public class GetTalentRequestValidator : AbstractValidator<GetTalentRequest>
    {
        public GetTalentRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
