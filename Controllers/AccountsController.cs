using AutoMapper;
using BudgetingApp.Models;
using BudgetingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BudgetingApp.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountTypeRepository accountTypeRepository;
        private readonly IServiceUsers serviceUsers;
        private readonly IAccountsRepository accountsrepository;
        private readonly IMapper mapper;
        private readonly ITransactionsRepository transactionsRepository;
        private readonly IReportsService reportsService;

        public AccountsController(IAccountTypeRepository accountTypeRepository, IServiceUsers serviceUsers, IAccountsRepository accountsrepository,
            IMapper mapper, ITransactionsRepository transactionsRepository, IReportsService reportsService)
        {
            this.accountTypeRepository = accountTypeRepository;
            this.serviceUsers = serviceUsers;
            this.accountsrepository = accountsrepository;
            this.mapper = mapper;
            this.transactionsRepository = transactionsRepository;
            this.reportsService = reportsService;
        }

        public async Task<IActionResult> Index()
        {
            var userid = serviceUsers.GetUserID();
            var accountypeacount = await accountsrepository.accountFind(userid);

            var model = accountypeacount.GroupBy(x => x.AccountType)
                                        .Select(group => new IndexModel
                                        {
                                            AccountTypeIndex = group.Key,
                                            Accounts = group.AsEnumerable()


                                        }).ToList();

            return View(model);
        }

        public async Task<IActionResult> Details(int id, int month, int year) {

            var userid = serviceUsers.GetUserID();
            var account = await accountsrepository.GetViaId(id, userid);
            if (account is null)
            {
                return RedirectToAction("Not_Found", "Home");

            }
            
            ViewBag.Account = account.Name;

            var model = await reportsService.GetReportTransactionsbyaccount(userid, id, month, year, ViewBag);




            return View(model);
        }

        [HttpGet]
        public async Task <IActionResult> Create()
        {
            var userid = serviceUsers.GetUserID();
            
            var model = new AccountCreation();
            model.AccountType = await (GetAccountType(userid));
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountCreation account)
        {
            var userid = serviceUsers.GetUserID();
            var accounttype = await accountTypeRepository.GetUserViaId(account.AccountTypeID, userid);
            if (accounttype is null)
            {
                return RedirectToAction("Not_Found", "Home");

            }
            if (!ModelState.IsValid)
            {

                account.AccountType = await GetAccountType(userid);
                return View(account);

            }

            await accountsrepository.Create(account);
            return RedirectToAction("Index");
        }
        private async Task<IEnumerable<SelectListItem>> GetAccountType(int userid) {
           
            var accounttype = await accountTypeRepository.GetAccount(userid);

            return accounttype.Select(x => new SelectListItem(x.Name, x.Id.ToString()));


        }

        public async Task<IActionResult> Edit(int id)
        {
            var userid = serviceUsers.GetUserID();

            var account = await accountsrepository.GetViaId(id, userid);

            if (account is null)
            {

                return RedirectToAction("Not_found", "Home");

            }

            var model = mapper.Map<AccountCreation>(account);

            model.AccountType = await (GetAccountType(userid));

            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(AccountCreation accountCreation) {

            var userid = serviceUsers.GetUserID();

            var account = await accountsrepository.GetViaId(accountCreation.Id, userid);

            if (account is null)
            {

                return RedirectToAction("Not_found", "Home");

            }

            var accounttype = await accountTypeRepository.GetUserViaId(accountCreation.AccountTypeID, userid);

            if (accounttype is null)
            {
                return RedirectToAction("Not_found", "Home");

            }

            await accountsrepository.Update(accountCreation);
            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var userId = serviceUsers.GetUserID();
            var account = await accountsrepository.GetViaId(Id, userId);
            if (account is null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(int Id)
        {
            var userId = serviceUsers.GetUserID();
            var account = await accountsrepository.GetViaId(Id, userId);
            if (account is null)
            {
                return RedirectToAction("Not_Found", "Home");
            }
            await accountsrepository.Delete(Id);

            return RedirectToAction("Index");
        }
    }
}
