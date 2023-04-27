namespace BudgetingApp.Models
{
    public class TransactionsReport
    {

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<TransactionsbyDate> TransactionsGroup { get; set; }
        public decimal DepositBalance => TransactionsGroup.Sum(x => x.DepositBalance);
        public decimal DebitsBalance => TransactionsGroup.Sum(x => x.DebitsBalance);

        public decimal Total => DepositBalance - DebitsBalance;
        public class TransactionsbyDate {

            public DateTime TransactionDate { get; set; }
            public IEnumerable<Transaction> Transactions { get; set; }
            public decimal DepositBalance => Transactions.Where(x => x.OperationTypeID == OperationType.Income)
                .Sum(x => x.Amount);
            public decimal DebitsBalance => Transactions.Where(x => x.OperationTypeID == OperationType.Expense)
                .Sum(x => x.Amount);
        }
    }
}
