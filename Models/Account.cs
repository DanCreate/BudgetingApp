using BudgetingApp.Validation;
using System.ComponentModel.DataAnnotations;

namespace BudgetingApp.Models
{
    public class Account
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(maximumLength:50)]
        [FirstLetterUpper]

        public string Name { get; set; }
        [Display(Name = "Account Type")]
        public int AccountTypeID { get; set; }
        
        public decimal Balance { get; set; }

        [StringLength(maximumLength: 1000)]
        public string Description { get; set; }

        public string AccountType{ get; set; }
    }
}
