using FluentValidation;

namespace Services.Features.Settings.DocumentTypes.UseCases.Commands
{
    public class CreateDocumentTypeRequestValidator : AbstractValidator<CreateDocumentTypeRequest>
    {
        public CreateDocumentTypeRequestValidator()
        {
            RuleFor(p => p.ExternalCode).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
