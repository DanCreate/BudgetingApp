namespace BudgetingApp.Models
{
    public class GetTransactionsbyAccount
    {

        public int UserId { get; set; }
        public int AccountID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
