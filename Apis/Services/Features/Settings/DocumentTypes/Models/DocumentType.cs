using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Services.Features.Financials.Contracts.Models;
using Services.Features.Peoples.Customers.Models;
using Services.Features.Peoples.Sellers.Models;
using Services.Features.Peoples.Sponsors.Models;
using Services.Features.Peoples.Suppliers.Models;
using Services.Features.Peoples.Talents.Models;
using Services.Features.Settings.DocumentTypes.Models.Events;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Settings.DocumentTypes.Models
{
    [Table("DocumentTypes", Schema = "Settings")]
    public partial class DocumentType : BaseEntity
    {
        public DocumentType()
        {
            ContractDocuments = [];
            CustomerDocuments = [];
            SellerDocuments = [];
            SponsorDocuments = [];
            SupplierDocuments = [];
            TalentDocuments = [];
        }

        public DocumentType(
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
            ContractDocuments = [];
            CustomerDocuments = [];
            SellerDocuments = [];
            SponsorDocuments = [];
            SupplierDocuments = [];
            TalentDocuments = [];
        }

        public DocumentType(int id, DateTime editedAt, string editedBy, DateTime deletedAt)
        {
            Id = id;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
            ContractDocuments = [];
            CustomerDocuments = [];
            SellerDocuments = [];
            SponsorDocuments = [];
            SupplierDocuments = [];
            TalentDocuments = [];
        }

        [StringLength(256)]
        public string ExternalCode { get; set; }

        [Required]
        public string Description { get; set; }

        public string Picture { get; set; }

        [JsonIgnore]
        [InverseProperty(nameof(ContractDocument.DocumentType))]
        public virtual ICollection<ContractDocument> ContractDocuments { get; set; }

        [JsonIgnore]
        [InverseProperty(nameof(CustomerDocument.DocumentType))]
        public virtual ICollection<CustomerDocument> CustomerDocuments { get; set; }

        [JsonIgnore]
        [InverseProperty(nameof(SellerDocument.DocumentType))]
        public virtual ICollection<SellerDocument> SellerDocuments { get; set; }

        [JsonIgnore]
        [InverseProperty(nameof(SponsorDocument.DocumentType))]
        public virtual ICollection<SponsorDocument> SponsorDocuments { get; set; }

        [JsonIgnore]
        [InverseProperty(nameof(SupplierDocument.DocumentType))]
        public virtual ICollection<SupplierDocument> SupplierDocuments { get; set; }

        [JsonIgnore]
        [InverseProperty(nameof(TalentDocument.DocumentType))]
        public virtual ICollection<TalentDocument> TalentDocuments { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new DocumentTypeCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new DocumentTypeEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new DocumentTypeRemovedEvent(Id));
        }
    }
}
