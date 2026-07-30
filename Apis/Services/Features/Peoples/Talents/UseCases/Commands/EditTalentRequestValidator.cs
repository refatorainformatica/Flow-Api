using FluentValidation;
using Services.Features.Peoples.Talents.Models;
using Services.Features.Peoples.Talents.Repositories;

namespace Services.Features.Peoples.Talents.UseCases.Commands
{
    public class EditTalentRequestValidator : AbstractValidator<EditTalentRequest>
    {
        private readonly TalentDbContext _talentDbContext;

        public EditTalentRequestValidator(TalentDbContext talentDbContext)
        {
            _talentDbContext = talentDbContext;

            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Name).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.AddressLine1).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => new { p.Id, p.Documents })
                .Must((x, cancellation) => DocumentAlreadyRegistered(x.Id, x.Documents))
                .WithMessage("Document already registered");

            RuleFor(p => new { p.Id, p.Name })
                .Must((x, cancellation) => NameAlreadyRegistered(x.Id, x.Name))
                .WithMessage("Name already registered");

            RuleFor(p => new { p.Id, p.Email })
                .Must((x, cancellation) => EmailAlreadyRegistered(x.Id, x.Email))
                .WithMessage("Email already registered");
        }

        private bool DocumentAlreadyRegistered(
            int id,
            IEnumerable<TalentRequest.TalentDocumentRequest> documents
        )
        {
            var found = false;

            documents
                .ToList()
                .ForEach(document =>
                {
                    var response = _talentDbContext.Talents.Where(talent =>
                        talent.Documents.Any(d =>
                            d.DocumentTypeId == document.DocumentTypeId
                            && d.EnrollmentCode == document.EnrollmentCode
                        )
                        && talent.Id != id
                    );

                    if (response.Any())
                    {
                        found = true;
                    }
                });

            return !found;
        }

        private bool NameAlreadyRegistered(int id, string name)
        {
            var response = _talentDbContext.Talents.Where(talent =>
                talent.Id != id && talent.Name.Equals(name)
            );
            return !response.Any();
        }

        private bool EmailAlreadyRegistered(int id, string email)
        {
            var response = _talentDbContext.Talents.Where(talent =>
                talent.Id != id && talent.Email.Equals(email)
            );
            return !response.Any();
        }
    }
}
