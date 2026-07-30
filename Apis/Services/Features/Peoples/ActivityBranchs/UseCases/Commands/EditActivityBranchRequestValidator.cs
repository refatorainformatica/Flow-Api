using FluentValidation;

namespace Services.Features.Peoples.ActivityBranchs.UseCases.Commands
{
    public class EditActivityBranchRequestValidator : AbstractValidator<EditActivityBranchRequest>
    {
        public EditActivityBranchRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
