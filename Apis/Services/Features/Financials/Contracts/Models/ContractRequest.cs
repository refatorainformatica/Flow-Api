namespace Services.Features.Financials.Contracts.Models
{
    public class ContractRequest
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int SupplierId { get; set; }
        public int ContractTypeId { get; set; }
        public int ContractStateId { get; set; }
        public decimal ContractBaseValue { get; set; }
        public decimal ContractValue { get; set; }
        public int NumberOfWorkingHours { get; set; }
        public bool OwnEquipment { get; set; }
        public string LeaderName { get; set; }
        public bool RemoteJob { get; set; }
        public int BankId { get; set; }
        public string BankAgency { get; set; }
        public string BankAccount { get; set; }
        public string PixKey { get; set; }
        public string BusinessUnit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Picture { get; set; }
        public ICollection<ContractDocumentRequest> Documents { get; set; } = [];
        public ICollection<ContractSubscriptionRequest> Subscriptions { get; set; } = [];

        public class ContractDocumentRequest
        {
            public int? Id { get; set; }
            public int DocumentTypeId { get; set; }
            public int CustomerId { get; set; }
            public string ExternalCode { get; set; }
            public string EnrollmentCode { get; set; }
            public DateTime EnrollmentDate { get; set; }
            public string File { get; set; }
            public string Picture { get; set; }
            public DateTime? DeletedAt { get; set; }
        }

        public class ContractSubscriptionRequest
        {
            public string SubscriptionCode { get; set; }
            public DateTime SubscriptionDate { get; set; }
            public string SubscriptionUser { get; set; }
        }
    }
}
