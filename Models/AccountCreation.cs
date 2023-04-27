using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetingApp.Models
{
    public class AccountCreation: Account
    {
        public IEnumerable<SelectListItem> AccountType { get; set; }
    }
}
