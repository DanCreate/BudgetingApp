namespace BudgetingApp.Models
{
    public class GetTransactionsbyUserParameter
    {

        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
