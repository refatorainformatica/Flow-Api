using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Financials.Banks.Models.Events;
using Services.Features.Financials.Contracts.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Banks.Models
{
    [Table("Banks", Schema = "Financials")]
    public partial class Bank : BaseEntity
    {
        [StringLength(256)]
        public string ExternalCode { get; set; }

        [Required]
        [StringLength(256)]
        public string Description { get; set; }

        public string Picture { get; set; }

        [InverseProperty(nameof(Contract.Bank))]
        public virtual ICollection<Contract> Contracts { get; set; }

        public Bank()
        {
            Contracts = [];
        }

        public Bank(
            int id,
            string externalCode,
            string description,
            string picture,
            DateTime createdAt,
            string createdBy,
            DateTime editedAt,
            string editedBy
        )
        {
            Id = id;
            ExternalCode = externalCode;
            Description = description;
            Picture = picture;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            EditedAt = editedAt;
            EditedBy = editedBy;
            Contracts = [];
        }

        public Bank(int id, DateTime editedAt, string editedBy, DateTime deletedAt)
        {
            Id = id;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
            Contracts = [];
        }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new BankCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new BankEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new BankRemovedEvent(Id));
        }
    }
}
