using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Peoples.ProfessionalProfiles.Models.Events;
using Services.Features.Projects.Allocations;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.ProfessionalProfiles.Models
{
    [Table("ProfessionalProfiles", Schema = "Peoples")]
    public partial class ProfessionalProfile : BaseEntity
    {
        public ProfessionalProfile()
        {
            Allocations = [];
        }

        public ProfessionalProfile(
            int id,
            string description,
            decimal hourlyValue,
            decimal overtimeValue,
            decimal additionalHourlyValue,
            string picture,
            DateTime createdAt,
            string createdBy,
            DateTime editedAt,
            string editedBy,
            DateTime? deletedAt = null
        )
        {
            Id = id;
            Description = description;
            HourlyValue = hourlyValue;
            OvertimeValue = overtimeValue;
            AdditionalHourlyValue = additionalHourlyValue;
            Picture = picture;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
            Allocations = [];
        }

        public ProfessionalProfile(int id, DateTime editedAt, string editedBy, DateTime deletedAt)
        {
            Id = id;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
            Allocations = [];
        }

        [StringLength(256)]
        public string Description { get; set; }

        [Column(TypeName = "money")]
        public decimal? HourlyValue { get; set; }

        [Column(TypeName = "money")]
        public decimal? OvertimeValue { get; set; }

        [Column(TypeName = "money")]
        public decimal? AdditionalHourlyValue { get; set; }

        public string Picture { get; set; }

        [InverseProperty(nameof(Allocation.ProfessionalProfile))]
        public virtual ICollection<Allocation> Allocations { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ProfessionalProfileCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ProfessionalProfileEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ProfessionalProfileRemovedEvent(Id));
        }
    }
}
