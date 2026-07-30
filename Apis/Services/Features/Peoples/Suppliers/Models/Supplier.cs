using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Services.Features.Financials.Contracts.Models;
using Services.Features.Financials.CurrencyTypes.Models;
using Services.Features.Financials.Invoices.Models;
using Services.Features.Peoples.ActivityBranchs.Models;
using Services.Features.Peoples.JuridicalNatures.Models;
using Services.Features.Peoples.Suppliers.Models.Events;
using Services.Features.Peoples.Talents.Models;
using Services.Features.Projects.Allocations;
using Services.Features.Projects.Timesheets;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Suppliers.Models
{
    [Table("Suppliers", Schema = "Peoples")]
    public partial class Supplier : BaseEntity
    {
        public Supplier()
        {
            ProjectAllocations = [];
            Invoices = [];
            Documents = [];
            Timesheets = [];
            Contracts = [];
        }

        public Supplier(
            int id,
            string companyName,
            string companyBusinessName,
            int? juridicalNatureId,
            DateTime? openingDate,
            DateTime? closingDate,
            int? activityBranchId,
            int? paymentingCurrencyTypeId,
            string addressLine1,
            string addressLine2,
            string email,
            string phoneNumber,
            string picture,
            int? talentId,
            DateTime createdAt,
            string createdBy,
            DateTime editedAt,
            string editedBy
        )
        {
            Id = id;
            CompanyName = companyName;
            CompanyBusinessName = companyBusinessName;
            JuridicalNatureId = juridicalNatureId;
            OpeningDate = openingDate;
            ClosingDate = closingDate;
            ActivityBranchId = activityBranchId;
            PaymentingCurrencyTypeId = paymentingCurrencyTypeId;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            Email = email;
            PhoneNumber = phoneNumber;
            Picture = picture;
            TalentId = talentId;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            EditedAt = editedAt;
            EditedBy = editedBy;
            ProjectAllocations = [];
            Invoices = [];
            Documents = [];
            Timesheets = [];
            Contracts = [];
        }

        public Supplier(int id, DateTime editedAt, string editedBy, DateTime deletedAt)
        {
            Id = id;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
            ProjectAllocations = [];
            Invoices = [];
            Documents = [];
            Timesheets = [];
            Contracts = [];
        }

        [StringLength(256)]
        public string CompanyName { get; set; }

        [StringLength(256)]
        public string CompanyBusinessName { get; set; }

        public int? JuridicalNatureId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? OpeningDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ClosingDate { get; set; }

        public int? ActivityBranchId { get; set; }

        public int? PaymentingCurrencyTypeId { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [StringLength(30)]
        public string PhoneNumber { get; set; }

        public string Picture { get; set; }

        public int? TalentId { get; set; }

        [Column(TypeName = "datetime")]
        [ForeignKey(nameof(ActivityBranchId))]
        [InverseProperty(nameof(ActivityBranch.PeopleSuppliers))]
        public virtual ActivityBranch ActivityBranch { get; set; }

        [InverseProperty(nameof(Contract.Supplier))]
        public virtual ICollection<Contract> Contracts { get; set; }

        [ForeignKey(nameof(PaymentingCurrencyTypeId))]
        [InverseProperty(nameof(CurrencyType.Suppliers))]
        public virtual CurrencyType PaymentingCurrencyType { get; set; }

        [ForeignKey(nameof(JuridicalNatureId))]
        [InverseProperty(nameof(JuridicalNature.Suppliers))]
        public virtual JuridicalNature JuridicalNature { get; set; }

        [ForeignKey(nameof(TalentId))]
        [InverseProperty(nameof(Talent.Suppliers))]
        public virtual Talent Talent { get; set; }

        [InverseProperty(nameof(Allocation.Supplier))]
        public virtual ICollection<Allocation> ProjectAllocations { get; set; }

        [DataMember]
        [InverseProperty(nameof(SupplierDocument.Supplier))]
        public virtual ICollection<SupplierDocument> Documents { get; set; }

        [InverseProperty(nameof(Invoice.Supplier))]
        public virtual ICollection<Invoice> Invoices { get; set; }

        [InverseProperty(nameof(Timesheet.Supplier))]
        public virtual ICollection<Timesheet> Timesheets { get; set; }

        public Supplier AddActivityBranch(ActivityBranch activityBranch)
        {
            ActivityBranch = activityBranch;
            ActivityBranchId = activityBranch?.Id;
            return this;
        }

        public Supplier AddPaymentingCurrencyType(CurrencyType currencyType)
        {
            PaymentingCurrencyType = currencyType;
            PaymentingCurrencyTypeId = currencyType?.Id;
            return this;
        }

        public Supplier AddJuridicalNature(JuridicalNature juridicalNature)
        {
            JuridicalNature = juridicalNature;
            JuridicalNatureId = juridicalNature?.Id;
            return this;
        }

        public Supplier AddTalent(Talent talent)
        {
            Talent = talent;
            TalentId = talent?.Id;
            return this;
        }

        public Supplier AddSupplierDocument(SupplierDocument supplierDocument)
        {
            Documents.Add(supplierDocument);
            supplierDocument.Supplier = this;
            return this;
        }

        public Supplier AddInvoice(Invoice invoice)
        {
            Invoices.Add(invoice);
            invoice.Supplier = this;
            return this;
        }

        public Supplier AddTimesheet(Timesheet timesheet)
        {
            Timesheets.Add(timesheet);
            timesheet.Supplier = this;
            return this;
        }

        public Supplier AddAllocation(Allocation allocation)
        {
            ProjectAllocations.Add(allocation);
            allocation.Supplier = this;
            return this;
        }

        public Supplier OnBeforeSave()
        {
            ActivityBranch = null;
            ProjectAllocations = null;
            PaymentingCurrencyType = null;
            Invoices = null;
            JuridicalNature = null;
            Talent = null;
            Timesheets = null;

            if (Documents.Count == 0)
            {
                return this;
            }

            foreach (var item in Documents)
            {
                item.CreatedBy = CreatedBy;
                item.CreatedAt = CreatedAt;
                item.DocumentType = null;
                item.EditedBy = EditedBy;
                item.EditedAt = EditedAt;
                item.Supplier = null;
            }

            return this;
        }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SupplierCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SupplierEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new SupplierRemovedEvent(Id));
        }
    }
}
