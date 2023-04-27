namespace BudgetingApp.Models
{
    public class IndexModel
    {

        public string AccountTypeIndex { get; set; }
        public IEnumerable<Account> Accounts { get; set; }
        public decimal Balance => Accounts.Sum(x => x.Balance);
    }
}
