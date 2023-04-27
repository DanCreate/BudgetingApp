using System.ComponentModel.DataAnnotations;

namespace BudgetingApp.Models
{
    public class Category
    {

        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Cannot be greater than {1} characters")]
        public string Name { get; set; }
        [Display (Name = "Operation type")]
        public OperationType OperationTypeID{ get; set; }
        public int UserID { get; set; }
    }
}
