using BudgetingApp.Validation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BudgetingApp.Models
{
    public class AccountOrderType
    {
        public int Id { get; set; }
        [Required]
        [FirstLetterUpper]
        [Remote(action: "AccountExists", controller: "AccountOrderType")]
        public string Name { get; set; }
        public int UserID { get; set; }
        public int OrderTransaction { get; set; }
    }
}
