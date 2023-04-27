using BudgetingApp.Models;

namespace BudgetingApp.Services
{

    public interface IReportsService
    {
        Task<TransactionsReport> GetReportTransactions(int userid, int month, int year, dynamic ViewBag);
        Task<TransactionsReport> GetReportTransactionsbyaccount(int userid, int accountID, int month, int year, dynamic ViewBag);
    }
    public class ReportsService: IReportsService
    {
        private readonly ITransactionsRepository transactionsRepository;
        private readonly HttpContext httpContext;

        public ReportsService(ITransactionsRepository transactionsRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.transactionsRepository = transactionsRepository;
            this.httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<TransactionsReport> GetReportTransactions(int userid, int month, int year, dynamic ViewBag)
        {
            (DateTime StartDate, DateTime EndDate) = GenerateStartDateEndDate(month, year);

            var parameter = new GetTransactionsbyUserParameter()
            {
                UserId = userid,
                StartDate = StartDate,
                EndDate = EndDate


            };

            var transactions = await transactionsRepository.GetbyUserId(parameter);

            var model = GenerateReportTransactions(StartDate, EndDate, transactions);
            ViewBagAsignValue(ViewBag, StartDate);

            return model;

        }

        public async Task<TransactionsReport> GetReportTransactionsbyaccount(int userid, int accountID, int month, int year, dynamic ViewBag)
        {

            (DateTime StartDate, DateTime EndDate) = GenerateStartDateEndDate(month, year);

            var gettransactionsbyaccount = new GetTransactionsbyAccount()
            {
                AccountID = accountID,
                UserId = userid,
                StartDate = StartDate,
                EndDate = EndDate



            };

            var transactions = await transactionsRepository.GetbyAccountId(gettransactionsbyaccount);
            var model = GenerateReportTransactions(StartDate, EndDate, transactions);
            ViewBagAsignValue(ViewBag, StartDate);

            return model;


        }

        private void ViewBagAsignValue(dynamic ViewBag, DateTime StartDate)
        {
            ViewBag.previousMonth = StartDate.AddMonths(-1).Month;
            ViewBag.previousYear = StartDate.AddMonths(-1).Year;
            ViewBag.monthAfter = StartDate.AddMonths(1).Month;
            ViewBag.yearAfter = StartDate.AddMonths(1).Year;
            ViewBag.urlReturn = httpContext.Request.Path + httpContext.Request.QueryString;
        }

        private static TransactionsReport GenerateReportTransactions(DateTime StartDate, DateTime EndDate, IEnumerable<Transaction> transactions)
        {
            var model = new TransactionsReport();


            var transactionsbydate = transactions.OrderByDescending(x => x.TransactionDate)
                .GroupBy(x => x.TransactionDate)
                .Select(group => new TransactionsReport.TransactionsbyDate()
                {
                    TransactionDate = group.Key,
                    Transactions = group.AsEnumerable()

                });

            model.TransactionsGroup = transactionsbydate;
            model.StartDate = StartDate;
            model.EndDate = EndDate;
            return model;
        }

        private (DateTime StartDate, DateTime EndDate) GenerateStartDateEndDate(int month, int year)
        {

            DateTime StartDate;
            DateTime EndDate;

            if (month <= 0 || month > 12 || year <= 1900)
            {
                var today = DateTime.Today;
                StartDate = new DateTime(today.Year, today.Month, 1);
            }
            else
            {
                StartDate = new DateTime(year, month, 1);


            }
            EndDate = StartDate.AddMonths(1).AddDays(-1);

            return (StartDate, EndDate);


        }
        
    }
}
