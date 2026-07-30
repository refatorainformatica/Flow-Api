using FluentValidation;

namespace Services.Features.Peoples.Talents.UseCases.Queries
{
    public class GetByIdTalentRequestValidator : AbstractValidator<GetByIdTalentRequest>
    {
        public GetByIdTalentRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
