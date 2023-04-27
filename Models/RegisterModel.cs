using System.ComponentModel.DataAnnotations;

namespace BudgetingApp.Models
{
    public class RegisterModel
    {

        [Required(ErrorMessage ="{0} is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string Password { get; set; }    
    }
}
