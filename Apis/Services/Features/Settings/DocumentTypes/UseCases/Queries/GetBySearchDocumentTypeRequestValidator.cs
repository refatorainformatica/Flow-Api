using FluentValidation;

namespace Services.Features.Settings.DocumentTypes.UseCases.Queries
{
    public class GetBySearchDocumentTypeRequestValidator
        : AbstractValidator<GetBySearchDocumentTypeRequest>
    {
        public GetBySearchDocumentTypeRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
