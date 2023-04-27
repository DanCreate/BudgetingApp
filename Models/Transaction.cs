using System.ComponentModel.DataAnnotations;

namespace BudgetingApp.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        [Display(Name = "Transaction Date")]
        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; } = DateTime.Today;
        public decimal Amount { get; set; }
        [StringLength(maximumLength:1000, ErrorMessage = "Memo must be under {1} characters")]
        public string Memo { get; set; }
        [Range(1, maximum: int.MaxValue, ErrorMessage = "Must select a category")]
        [Display(Name = "Account")]
        public int AccountID { get; set; }
        [Display(Name ="Category")]
        [Range(1, maximum: int.MaxValue, ErrorMessage = "Must select a category")]
        public int CategoryID { get; set; }

        [Display(Name = "Operation Type")]
        public OperationType OperationTypeID { get; set; } = OperationType.Income;

        public string Account { get; set; }
        public string Category { get; set; }



    }
}
