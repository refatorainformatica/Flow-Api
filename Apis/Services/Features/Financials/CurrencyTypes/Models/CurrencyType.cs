using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Financials.CashFlows.Models;
using Services.Features.Financials.CurrencyTypes.Models.Events;
using Services.Features.Peoples.Sponsors.Models;
using Services.Features.Peoples.Suppliers.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.CurrencyTypes.Models
{
    [Table("CurrencyTypes", Schema = "Settings")]
    public partial class CurrencyType : BaseEntity
    {
        public CurrencyType()
        {
            CashFlows = [];
            Sponsors = [];
            Suppliers = [];
        }

        public CurrencyType(
            int id,
            string description,
            string picture,
            DateTime createdAt,
            string createdBy,
            DateTime editedAt,
            string editedBy
        )
        {
            Id = id;
            Description = description;
            Picture = picture;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            EditedAt = editedAt;
            EditedBy = editedBy;
        }

        [Required]
        [StringLength(256)]
        public string Description { get; set; }

        public string Picture { get; set; }

        [InverseProperty(nameof(CashFlow.CurrencyType))]
        public virtual ICollection<CashFlow> CashFlows { get; set; }

        [InverseProperty(nameof(Sponsor.PaymentingCurrencyType))]
        public virtual ICollection<Sponsor> Sponsors { get; set; }

        [InverseProperty(nameof(Supplier.PaymentingCurrencyType))]
        public virtual ICollection<Supplier> Suppliers { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new CurrencyTypeCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new CurrencyTypeEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new CurrencyTypeRemovedEvent(Id));
        }
    }
}
