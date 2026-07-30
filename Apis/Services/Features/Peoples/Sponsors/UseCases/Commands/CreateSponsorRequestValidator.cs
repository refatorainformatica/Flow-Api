using FluentValidation;
using Services.Features.Peoples.Sponsors.Models;
using Services.Features.Peoples.Sponsors.Repositories;

namespace Services.Features.Peoples.Sponsors.UseCases.Commands
{
    public class CreateSponsorRequestValidator : AbstractValidator<CreateSponsorRequest>
    {
        private readonly SponsorDbContext _sponsorDbContext;

        public CreateSponsorRequestValidator(SponsorDbContext sponsorDbContext)
        {
            _sponsorDbContext = sponsorDbContext;

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
            IEnumerable<SponsorRequest.SponsorDocumentRequest> documents
        )
        {
            var found = false;

            documents
                .ToList()
                .ForEach(document =>
                {
                    var response = _sponsorDbContext.Sponsors.Where(sponsor =>
                        sponsor.Documents.Any(d =>
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
            var response = _sponsorDbContext.Sponsors.Where(sponsor => sponsor.Name.Equals(name));
            return !response.Any();
        }

        private bool EmailAlreadyRegistered(string email)
        {
            var response = _sponsorDbContext.Sponsors.Where(sponsor => sponsor.Email.Equals(email));
            return !response.Any();
        }
    }
}
