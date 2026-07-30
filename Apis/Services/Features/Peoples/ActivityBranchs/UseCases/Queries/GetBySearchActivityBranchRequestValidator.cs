using FluentValidation;

namespace Services.Features.Peoples.ActivityBranchs.UseCases.Queries
{
    public class GetBySearchActivityBranchRequestValidator
        : AbstractValidator<GetBySearchActivityBranchRequest>
    {
        public GetBySearchActivityBranchRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
