using FluentValidation;

namespace Services.Features.Peoples.ActivityBranchs.UseCases.Commands
{
    public class RemoveActivityBranchRequestValidator
        : AbstractValidator<RemoveActivityBranchRequest>
    {
        public RemoveActivityBranchRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
