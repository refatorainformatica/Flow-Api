using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Financials.Banks.Models;
using Services.Features.Financials.Contracts.Models.Events;
using Services.Features.Financials.ContractStates.Models;
using Services.Features.Financials.ContractTypes.Models;
using Services.Features.Peoples.Suppliers.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Contracts.Models
{
    [Table("Contracts", Schema = "Financials")]
    public partial class Contract : BaseEntity
    {
        public Contract()
        {
            ContractDocuments = [];
            ContractSubscriptions = [];
        }

        public Contract(
            int id,
            string description,
            int supplierId,
            int contractTypeId,
            int contractStateId,
            decimal contractBaseValue,
            decimal contractValue,
            int numberOfWorkingHours,
            bool ownEquipment,
            string leaderName,
            bool remoteJob,
            int bankId,
            string bankAgency,
            string bankAccount,
            string pixKey,
            string businessUnit,
            DateTime startDate,
            DateTime? endDate,
            string picture,
            DateTime createdAt,
            string createdBy,
            DateTime editedAt,
            string editedBy
        )
        {
            Id = id;
            Description = description;
            SupplierId = supplierId;
            ContractTypeId = contractTypeId;
            ContractStateId = contractStateId;
            ContractBaseValue = contractBaseValue;
            ContractValue = contractValue;
            NumberOfWorkingHours = numberOfWorkingHours;
            OwnEquipment = ownEquipment;
            LeaderName = leaderName;
            RemoteJob = remoteJob;
            BankId = bankId;
            BankAgency = bankAgency;
            BankAccount = bankAccount;
            PixKey = pixKey;
            BusinessUnit = businessUnit;
            StartDate = startDate;
            EndDate = endDate;
            Picture = picture;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            EditedAt = editedAt;
            EditedBy = editedBy;
            ContractDocuments = [];
            ContractSubscriptions = [];
        }

        public Contract(int id, DateTime editedAt, string editedBy, DateTime deletedAt)
        {
            Id = id;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
            ContractDocuments = [];
            ContractSubscriptions = [];
        }

        [Required]
        public string Description { get; set; }

        public int? SupplierId { get; set; }

        public int? ContractTypeId { get; set; }

        public int? ContractStateId { get; set; }

        [Column(TypeName = "money")]
        public decimal? ContractBaseValue { get; set; }

        [Column(TypeName = "money")]
        public decimal? ContractValue { get; set; }

        public int? NumberOfWorkingHours { get; set; }

        public bool? OwnEquipment { get; set; }

        [StringLength(255)]
        public string LeaderName { get; set; }

        public bool? RemoteJob { get; set; }

        public int? BankId { get; set; }

        [StringLength(15)]
        public string BankAgency { get; set; }

        [StringLength(15)]
        public string BankAccount { get; set; }

        [StringLength(255)]
        public string PixKey { get; set; }

        [StringLength(255)]
        public string BusinessUnit { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public string Picture { get; set; }

        [StringLength(15)]
        public string RenewalCode { get; set; }

        [ForeignKey(nameof(BankId))]
        [InverseProperty(nameof(Bank.Contracts))]
        public virtual Bank Bank { get; set; }

        [ForeignKey(nameof(ContractStateId))]
        [InverseProperty(nameof(ContractState.Contracts))]
        public virtual ContractState ContractState { get; set; }

        [ForeignKey(nameof(ContractTypeId))]
        [InverseProperty(nameof(ContractType.Contracts))]
        public virtual ContractType ContractType { get; set; }

        [ForeignKey(nameof(SupplierId))]
        [InverseProperty(nameof(Supplier.Contracts))]
        public virtual Supplier Supplier { get; set; }

        [InverseProperty(nameof(ContractDocument.Contract))]
        public virtual ICollection<ContractDocument> ContractDocuments { get; set; }

        [InverseProperty(nameof(ContractSubscription.Contract))]
        public virtual ICollection<ContractSubscription> ContractSubscriptions { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ContractCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ContractEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ContractRemovedEvent(Id));
        }
    }
}
