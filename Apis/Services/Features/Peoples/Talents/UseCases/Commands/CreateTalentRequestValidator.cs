using FluentValidation;
using Services.Features.Peoples.Talents.Models;
using Services.Features.Peoples.Talents.Repositories;

namespace Services.Features.Peoples.Talents.UseCases.Commands
{
    public class CreateTalentRequestValidator : AbstractValidator<CreateTalentRequest>
    {
        private readonly TalentDbContext _talentDbContext;

        public CreateTalentRequestValidator(TalentDbContext talentDbContext)
        {
            _talentDbContext = talentDbContext;

            RuleFor(p => p.Name).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.AddressLine1).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => new { p.Documents })
                .Must((x, cancellation) => DocumentAlreadyRegistered(x.Documents))
                .WithMessage("Document already registered");

            RuleFor(p => new { p.Name })
                .Must((x, cancellation) => NameAlreadyRegistered(x.Name))
                .WithMessage("Name already registered");

            RuleFor(p => new { p.Email })
                .Must((x, cancellation) => EmailAlreadyRegistered(x.Email))
                .WithMessage("Email already registered");
        }

        private bool DocumentAlreadyRegistered(
            IEnumerable<TalentRequest.TalentDocumentRequest> documents
        )
        {
            var found = false;

            documents
                .ToList()
                .ForEach(document =>
                {
                    var response = _talentDbContext.Talents.Where(customer =>
                        customer.Documents.Any(d =>
                            d.DocumentTypeId == document.DocumentTypeId
                            && d.EnrollmentCode == document.EnrollmentCode
                        )
                    );

                    if (response.Any())
                    {
                        found = true;
                    }
                });

            return !found;
        }

        private bool NameAlreadyRegistered(string name)
        {
            var response = _talentDbContext.Talents.Where(customer => customer.Name.Equals(name));
            return !response.Any();
        }

        private bool EmailAlreadyRegistered(string email)
        {
            var response = _talentDbContext.Talents.Where(customer => customer.Email.Equals(email));
            return !response.Any();
        }
    }
}
