using FluentValidation;

namespace Services.Features.Settings.DocumentTypes.UseCases.Commands
{
    public class EditDocumentTypeRequestValidator : AbstractValidator<EditDocumentTypeRequest>
    {
        public EditDocumentTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
