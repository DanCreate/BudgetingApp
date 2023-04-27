using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace BudgetingApp.Models
{
    public class User
    {

        public int Id { get; set; }
        public string Email { get; set; }
        public string EmailNormalized { get; set; }
        public string PasswordHash{ get; set; }
    }
}
