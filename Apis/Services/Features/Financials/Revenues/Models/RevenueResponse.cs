using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Revenues.Models
{
    public class RevenueResponse : BaseResponse
    {
        public int InvoiceId { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime DateOfDue { get; set; }
        public DateTime DateOfPayment { get; set; }
        public int InstallmentNumber { get; set; }
        public int TotalNumberOfInstallments { get; set; }
        public decimal PaymentValue { get; set; }
        public decimal PaymentDiscountValue { get; set; }
        public decimal TotalPaymentValue { get; set; }
        public string BarCode { get; set; }
        public string Observation { get; set; }
        public int CostCenterId { get; set; }
        public int PaymentStateId { get; set; }
        public int RevenueTypeId { get; set; }
    }
}
