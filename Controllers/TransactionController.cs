using AutoMapper;
using BudgetingApp.Models;
using BudgetingApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;


namespace BudgetingApp.Controllers
{
   
    public class TransactionController : Controller
    {
        private readonly IServiceUsers serviceUsers;
        private readonly IAccountsRepository accountsRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ITransactionsRepository transactionsRepository;
        private readonly IMapper mapper;
        private readonly IReportsService reportsService;

        public TransactionController(IServiceUsers serviceUsers, IAccountsRepository accountsRepository, ICategoryRepository categoryRepository,
                                     ITransactionsRepository transactionsRepository, IMapper mapper, IReportsService reportsService)
        {
            this.serviceUsers = serviceUsers;
            this.accountsRepository = accountsRepository;
            this.categoryRepository = categoryRepository;
            this.transactionsRepository = transactionsRepository;
            this.mapper = mapper;
            this.reportsService = reportsService;
        }
        
        public async Task <IActionResult> Index(int month, int year)
        {
            var userid = serviceUsers.GetUserID();
            var model = await reportsService.GetReportTransactions(userid, month, year, ViewBag);

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var userid = serviceUsers.GetUserID();
            var model = new TransactionModel();
            model.Accounts = await GetAccount(userid);
            model.Categories = await GetCategories(userid, model.OperationTypeID);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TransactionModel model)
        {
            var userid = serviceUsers.GetUserID();

            if (!ModelState.IsValid)
            {
                model.Accounts = await GetAccount(userid);
                model.Categories = await GetCategories(userid, model.OperationTypeID);
                return View(model);

            }


            var accountalreadyexists = await accountsRepository.GetViaId(model.AccountID, userid);

            if (accountalreadyexists is null)
            {


                return RedirectToAction("Not_found", "Home");
            }

            var category = await categoryRepository.GetViaId(model.CategoryID, userid);

            if (category is null)
            {


                return RedirectToAction("Not_found", "Home");
            }

            model.UserID = userid;

            if (model.OperationTypeID == OperationType.Expense)
            {
                model.Amount *= -1;
            }

            await transactionsRepository.Create(model);
            return RedirectToAction("Index");

        }

        private async Task<IEnumerable<SelectListItem>> GetAccount(int userId)
        {

            var account = await accountsRepository.accountFind(userId);

            return account.Select(x => new SelectListItem(x.Name, x.Id.ToString()));

        }

        private async Task<IEnumerable<SelectListItem>> GetCategories(int userid, OperationType operationType)
        {
            var categories = await categoryRepository.GetOperationType(userid, operationType);
            return categories.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }

        [HttpPost]
        public async Task<IActionResult> GetCategories([FromBody] OperationType operationType)
        {
            var userid = serviceUsers.GetUserID();
            var categories = await GetCategories(userid, operationType);

            return Ok(categories);




        }

        [HttpGet]

        public async Task<IActionResult> Edit(int id, string urlReturn = null)
        {
            var userid = serviceUsers.GetUserID();

            var transaction = await transactionsRepository.GetViaId(id, userid);

            if (transaction is null)
            {

                return RedirectToAction("Not_found", "Home");
            }
            var model = mapper.Map<UpdateTransactionModel>(transaction);

            model.PreviousAmount = model.Amount;

            if (model.OperationTypeID == OperationType.Expense)
            {
                model.PreviousAmount = model.Amount * -1;
            }

            model.PreviousAccountId = transaction.AccountID;
            model.Categories = await GetCategories(userid, transaction.OperationTypeID);
            model.Accounts = await GetAccount(userid);
            model.UrlReturn = urlReturn;
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateTransactionModel model)
        {
            var userid = serviceUsers.GetUserID();
            if (!ModelState.IsValid)
            {
                model.Accounts = await GetAccount(userid);
                model.Categories = await GetCategories(userid, model.OperationTypeID);

                return View(model);

            }



            var account = await accountsRepository.GetViaId(model.AccountID, userid);

            if (account is null)
            {

                return RedirectToAction("Not_found", "Home");

            }
            var category = await categoryRepository.GetViaId(model.CategoryID, userid);

            if (category is null)
            {

                return RedirectToAction("Not_found", "Home");

            }
            var transaction = mapper.Map<Transaction>(model);

            if (model.OperationTypeID == OperationType.Expense)
            {
                transaction.Amount *= -1;

            }


            await transactionsRepository.Update(transaction, model.PreviousAmount, model.PreviousAccountId);

            if (string.IsNullOrEmpty(model.UrlReturn))
            {
                return RedirectToAction("Index");
            }
            else
            {

                return LocalRedirect(model.UrlReturn);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id, string urlReturn = null)
        {
            var userId = serviceUsers.GetUserID();
            var transaction = await transactionsRepository.GetViaId(Id, userId);
            if (transaction is null)
            {
                return RedirectToAction("Not_Found", "Home");
            }
            await transactionsRepository.Delete(Id);

            if (string.IsNullOrEmpty(urlReturn))
            {
                return RedirectToAction("Index");
            }
            else
            {

                return LocalRedirect(urlReturn);
            }

        }
    }
}
