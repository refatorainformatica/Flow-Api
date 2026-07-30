using Services.Features.Financials.Banks.Models;
using Services.Features.Financials.ContractStates.Models;
using Services.Features.Financials.ContractTypes.Models;
using Services.Features.Peoples.Suppliers.Models;
using Services.Features.Settings.DocumentTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Contracts.Models
{
    public class ContractResponse : BaseResponse
    {
        public string Description { get; set; }
        public int? SupplierId { get; set; }
        public SupplierResponse Supplier { get; set; }
        public int? ContractTypeId { get; set; }
        public ContractTypeResponse ContractType { get; set; }
        public int? ContractStateId { get; set; }
        public ContractStateResponse ContractState { get; set; }
        public decimal ContractBaseValue { get; set; }
        public decimal ContractValue { get; set; }
        public int NumberOfWorkingHours { get; set; }
        public bool OwnEquipment { get; set; }
        public string LeaderName { get; set; }
        public bool RemoteJob { get; set; }
        public int? BankId { get; set; }
        public BankResponse Bank { get; set; }
        public string BankAgency { get; set; }
        public string BankAccount { get; set; }
        public string PixKey { get; set; }
        public string BusinessUnit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string RenewalCode { get; set; }
        public ICollection<ContractDocumentResponse> Documents { get; set; } = [];
        public ICollection<ContractSubscriptionResponse> Subscriptions { get; set; } = [];

        public class ContractDocumentResponse : BaseResponse
        {
            public int DocumentTypeId { get; set; }
            public DocumentTypeResponse DocumentType { get; set; }
            public int SponsorId { get; set; }
            public string ExternalCode { get; set; }
            public string EnrollmentCode { get; set; }
            public DateTime EnrollmentDate { get; set; }
            public string File { get; set; }
        }

        public class ContractSubscriptionResponse : BaseResponse
        {
            public string SubscriptionCode { get; set; }
            public DateTime SubscriptionDate { get; set; }
            public string SubscriptionUser { get; set; }
        }
    }
}
