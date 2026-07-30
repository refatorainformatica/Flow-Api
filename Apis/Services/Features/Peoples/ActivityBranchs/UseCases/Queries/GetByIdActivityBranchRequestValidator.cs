using FluentValidation;

namespace Services.Features.Peoples.ActivityBranchs.UseCases.Queries
{
    public class GetByIdActivityBranchRequestValidator
        : AbstractValidator<GetByIdActivityBranchRequest>
    {
        public GetByIdActivityBranchRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
