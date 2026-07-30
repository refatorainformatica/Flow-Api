using FluentValidation;

namespace Services.Features.Settings.DocumentTypes.UseCases.Commands
{
    public class RemoveDocumentTypeRequestValidator : AbstractValidator<RemoveDocumentTypeRequest>
    {
        public RemoveDocumentTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
