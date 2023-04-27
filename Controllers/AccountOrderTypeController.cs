using BudgetingApp.Models;
using BudgetingApp.Services;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Data.SqlClient;

namespace BudgetingApp.Controllers
{
    public class AccountOrderTypeController : Controller
    {
        private readonly IAccountTypeRepository accountTypeRepository;
        private readonly IServiceUsers serviceUsers;

        public AccountOrderTypeController(IAccountTypeRepository accountTypeRepository, IServiceUsers serviceUsers)
        {
            this.accountTypeRepository = accountTypeRepository;
            this.serviceUsers = serviceUsers;
        }

        public async Task <IActionResult>Index() {

            var userID = serviceUsers.GetUserID();
            var accountType = await accountTypeRepository.GetAccount(userID);
            return View(accountType);
        
        
        }

        public IActionResult Create()

        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AccountOrderType accountOrderType)
        {

            if (!ModelState.IsValid)
            {
                return View(accountOrderType);

            }
            accountOrderType.UserID = serviceUsers.GetUserID();

            var accountalreadyexists = await accountTypeRepository.Exists(accountOrderType.Name, accountOrderType.UserID);

            if (accountalreadyexists)
            {
                ModelState.AddModelError(nameof(accountOrderType.Name), $"Name {accountOrderType.Name} already exists.");

                return View(accountOrderType);
            }

            await accountTypeRepository.Create(accountOrderType);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        { 
        var userId = serviceUsers.GetUserID();
        var accounttype = await accountTypeRepository.GetUserViaId(id, userId);

            if (accounttype is null)
            {
                return RedirectToAction("Not_Found", "Home");

            }

            return View(accounttype);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(AccountOrderType accountOrderType) 
        {
            var userId = serviceUsers.GetUserID();
            var accountexists = await accountTypeRepository.GetUserViaId(accountOrderType.Id, userId);
            if (accountexists is null)
            {
                return RedirectToAction("Not_Found", "Home");
            }
            await accountTypeRepository.Update(accountOrderType);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete (int Id)
        {
            var userId = serviceUsers.GetUserID();
            var accountType = await accountTypeRepository.GetUserViaId(Id, userId);
            if (accountType is null)
            {
                return RedirectToAction("Not_Found", "Home");
            }
            
            return View(accountType);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccountType(int Id)
        {
            var userId = serviceUsers.GetUserID();
            var accountexists = await accountTypeRepository.GetUserViaId(Id, userId);
            if (accountexists is null)
            {
                return RedirectToAction("Not_Found", "Home");
            }
            await accountTypeRepository.Delete(Id);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult>AccountExists(string name) {

            var userID = serviceUsers.GetUserID();
            var alreadyexists = await accountTypeRepository.Exists(name, userID);

            if (alreadyexists)
            {
                return Json($"Name {name} already exists");
            }
            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> Order([FromBody] int[] ids)
        {
            var userid = serviceUsers.GetUserID();
            var accountType = await accountTypeRepository.GetAccount(userid);
            var idsAccountType = accountType.Select(x => x.Id);

            var idsAccountTypeNotUser = ids.Except(idsAccountType).ToList();


            if (idsAccountTypeNotUser.Count > 0)
            {
                return Forbid();

            }

            var accountTypeOrdered = ids.Select((value, index) => new AccountOrderType() { Id = value, OrderTransaction = index + 1 }).AsEnumerable();

            await accountTypeRepository.Order(accountTypeOrdered);


            return Ok();
        }
    }
}
