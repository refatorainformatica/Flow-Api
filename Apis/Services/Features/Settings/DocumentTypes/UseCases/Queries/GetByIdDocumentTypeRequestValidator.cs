using FluentValidation;

namespace Services.Features.Settings.DocumentTypes.UseCases.Queries
{
    public class GetByIdDocumentTypeRequestValidator : AbstractValidator<GetByIdDocumentTypeRequest>
    {
        public GetByIdDocumentTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
