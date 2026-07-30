namespace Services.Features.Financials.CashFlows.Models
{
    public class CashFlowRequest
    {
        public int Id { get; set; }
        public int YearExercise { get; set; }
        public int MonthExercise { get; set; }
        public int MovementTypeId { get; set; }
        public DateTime FinancialMovementDate { get; set; }
        public decimal FinancialMovementValue { get; set; }
        public decimal BalanceValue { get; set; }
        public int ExpenseId { get; set; }
        public int RevenueId { get; set; }
        public int CurrencyTypeId { get; set; }
        public string Picture { get; set; }
    }
}
