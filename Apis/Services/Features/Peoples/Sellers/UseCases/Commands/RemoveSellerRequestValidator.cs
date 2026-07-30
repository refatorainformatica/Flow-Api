using FluentValidation;

namespace Services.Features.Peoples.Sellers.UseCases.Commands
{
    public class RemoveSellerRequestValidator : AbstractValidator<RemoveSellerRequest>
    {
        public RemoveSellerRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
