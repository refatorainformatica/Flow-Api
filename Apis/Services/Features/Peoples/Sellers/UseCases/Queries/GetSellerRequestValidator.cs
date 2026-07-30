using FluentValidation;

namespace Services.Features.Peoples.Sellers.UseCases.Queries
{
    public class GetSellerRequestValidator : AbstractValidator<GetSellerRequest>
    {
        public GetSellerRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
