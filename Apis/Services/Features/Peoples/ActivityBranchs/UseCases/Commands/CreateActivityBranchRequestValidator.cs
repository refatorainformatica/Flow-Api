using FluentValidation;

namespace Services.Features.Peoples.ActivityBranchs.UseCases.Commands
{
    public class CreateActivityBranchRequestValidator
        : AbstractValidator<CreateActivityBranchRequest>
    {
        public CreateActivityBranchRequestValidator()
        {
            RuleFor(p => p.ExternalCode).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
