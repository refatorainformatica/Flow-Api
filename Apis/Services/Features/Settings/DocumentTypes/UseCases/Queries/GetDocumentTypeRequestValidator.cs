using FluentValidation;

namespace Services.Features.Settings.DocumentTypes.UseCases.Queries
{
    public class GetDocumentTypeRequestValidator : AbstractValidator<GetDocumentTypeRequest>
    {
        public GetDocumentTypeRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
