namespace BudgetingApp.Models
{
    public class UpdateTransactionModel: TransactionModel
    {
        public int PreviousAccountId { get; set; }   
        public decimal PreviousAmount { get; set; }
        public string UrlReturn { get; set; }
    }
}
