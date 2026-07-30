using FluentValidation;

namespace Services.Features.Peoples.ActivityBranchs.UseCases.Queries
{
    public class GetActivityBranchRequestValidator : AbstractValidator<GetActivityBranchRequest>
    {
        public GetActivityBranchRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
