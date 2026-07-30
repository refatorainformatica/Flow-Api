using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Peoples.Careers.Models;
using Services.Features.Peoples.EducationLevels.Models;
using Services.Features.Peoples.MaritalStates.Models;
using Services.Features.Peoples.Skills.Models;
using Services.Features.Peoples.Suppliers.Models;
using Services.Features.Peoples.Talents.Models.Events;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Talents.Models
{
    [Table("Talents", Schema = "Peoples")]
    public partial class Talent : BaseEntity
    {
        public Talent()
        {
            Skills = [];
            Suppliers = [];
            Documents = [];
        }

        public Talent(
            int id,
            string name,
            DateTime? birthDate,
            string birthPlace,
            string nationality,
            string naturalness,
            bool livesAbroad,
            bool remoteJob,
            short numberOfChildren,
            string fatherName,
            string motherName,
            int? careerId,
            decimal? grossIncomeValue,
            string consortName,
            int? maritalStateId,
            int? educationLevelId,
            string addressLine1,
            string addressLine2,
            string email,
            string corporateEmail,
            string phoneNumber,
            string emergencyContact,
            string linkedIn,
            bool fired,
            string resignationOpinion,
            string picture,
            DateTime createdAt,
            string createdBy,
            DateTime editedAt,
            string editedBy,
            DateTime? deletedAt = null
        )
        {
            Id = id;
            Name = name;
            BirthDate = birthDate;
            BirthPlace = birthPlace;
            Nationality = nationality;
            Naturalness = naturalness;
            LivesAbroad = livesAbroad;
            RemoteJob = remoteJob;
            NumberOfChildren = numberOfChildren;
            FatherName = fatherName;
            MotherName = motherName;
            CareerId = careerId;
            ValueOfServices = grossIncomeValue;
            ConsortName = consortName;
            MaritalStateId = maritalStateId;
            EducationLevelId = educationLevelId;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            Email = email;
            CorporateEmail = corporateEmail;
            PhoneNumber = phoneNumber;
            EmergencyContact = emergencyContact;
            LinkedIn = linkedIn;
            Fired = fired;
            ResignationOpinion = resignationOpinion;
            Picture = picture;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
            Skills = [];
            Suppliers = [];
            Documents = [];
        }

        public Talent(int id, DateTime editedAt, string editedBy, DateTime deletedAt)
        {
            Id = id;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
            Skills = [];
            Suppliers = [];
            Documents = [];
        }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BirthDate { get; set; }

        [StringLength(256)]
        public string BirthPlace { get; set; }

        [StringLength(256)]
        public string Nationality { get; set; }

        [StringLength(256)]
        public string Naturalness { get; set; }

        public bool LivesAbroad { get; set; }

        public bool RemoteJob { get; set; }

        public short NumberOfChildren { get; set; }

        [StringLength(256)]
        public string FatherName { get; set; }

        [StringLength(256)]
        public string MotherName { get; set; }

        public int? CareerId { get; set; }

        [Column(TypeName = "money")]
        public decimal? ValueOfServices { get; set; }

        [StringLength(256)]
        public string ConsortName { get; set; }

        public int? MaritalStateId { get; set; }

        public int? EducationLevelId { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [StringLength(256)]
        public string CorporateEmail { get; set; }

        [StringLength(30)]
        public string PhoneNumber { get; set; }

        [StringLength(256)]
        public string EmergencyContact { get; set; }

        [StringLength(256)]
        public string LinkedIn { get; set; }

        public bool Fired { get; set; }

        public string ResignationOpinion { get; set; }

        public string Picture { get; set; }

        [ForeignKey(nameof(CareerId))]
        [InverseProperty(nameof(Career.PeopleTalents))]
        public virtual Career Career { get; set; }

        [ForeignKey(nameof(EducationLevelId))]
        [InverseProperty(nameof(EducationLevel.Talents))]
        public virtual EducationLevel EducationLevel { get; set; }

        [ForeignKey(nameof(MaritalStateId))]
        [InverseProperty(nameof(MaritalState.Talents))]
        public virtual MaritalState MaritalState { get; set; }

        [InverseProperty(nameof(Skill.Talent))]
        public virtual ICollection<Skill> Skills { get; set; }

        [InverseProperty(nameof(Supplier.Talent))]
        public virtual ICollection<Supplier> Suppliers { get; set; }

        [InverseProperty(nameof(TalentDocument.Talent))]
        public virtual ICollection<TalentDocument> Documents { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new TalentCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new TalentEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new TalentRemovedEvent(Id));
        }
    }
}
