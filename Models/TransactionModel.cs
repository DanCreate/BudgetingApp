using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BudgetingApp.Models
{
    public class TransactionModel: Transaction
    {
        public IEnumerable <SelectListItem> Accounts { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
        
    }
}
