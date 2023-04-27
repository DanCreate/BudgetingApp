using BudgetingApp.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetingApp.Services
{

    public interface ITransactionsRepository
    {
        Task Create(Transaction transaction);
        Task Delete(int Id);
        Task<IEnumerable<Transaction>> GetbyAccountId(GetTransactionsbyAccount model);
        Task<IEnumerable<Transaction>> GetbyUserId(GetTransactionsbyUserParameter model);
        Task<Transaction> GetViaId(int id, int userid);
        Task Update(Transaction transaction, decimal previousAmount, int previousAccount);
    }
    public class TransactionsRepository: ITransactionsRepository
    {
        private readonly string connectionString;

        public TransactionsRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Create(Transaction transaction)
        {

            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>("Insert_Transaction", new {transaction.UserID,transaction.TransactionDate, transaction.Amount, transaction.CategoryID,
                                                                                        transaction.AccountID,transaction.Memo }, commandType: System.Data.CommandType.StoredProcedure);

            transaction.Id = id;

        }

        public async Task<IEnumerable<Transaction>> GetbyAccountId(GetTransactionsbyAccount model)
        {

            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Transaction>(
                @"SELECT t.Id, t.Amount, t.TransactionDate, c.Name as Category,
                cu.Name as Account, c.TransactionTypeID as OperationTypeID
                FROM Transactions t
                inner join Categories c
                ON c.Id = t.CategoryID
                inner join Account cu
                ON cu.Id = t.AccountID
                WHERE t.AccountID = @AccountID AND t.UserID = @UserId
                AND TransactionDate BETWEEN @StartDate and @EndDate", model );

        }

        public async Task<IEnumerable<Transaction>> GetbyUserId(GetTransactionsbyUserParameter model)
        {

            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Transaction>(
                @"SELECT t.Id, t.Amount, t.TransactionDate, c.Name as Category,
                cu.Name as Account, c.TransactionTypeID as OperationTypeID
                FROM Transactions t
                inner join Categories c
                ON c.Id = t.CategoryID
                inner join Account cu
                ON cu.Id = t.AccountID
                WHERE t.UserID = @UserId
                AND TransactionDate BETWEEN @StartDate and @EndDate ORDER BY t.TransactionDate DESC", model);

        }


        public async Task Update(Transaction transaction, decimal previousAmount, int PreviousAccountID)
        {

            using var connection = new SqlConnection(connectionString);
             await connection.ExecuteAsync("Update_Transactions", new
            {
                transaction.Id,
                transaction.TransactionDate,
                transaction.Amount,
                previousAmount,
                transaction.AccountID,
                 PreviousAccountID,
                transaction.CategoryID,
                transaction.Memo
                
                 
             }, commandType: System.Data.CommandType.StoredProcedure);

            

        }

        public async Task<Transaction> GetViaId(int id, int userid)
        {

            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Transaction>(
                @"SELECT Transactions.*, cat.TransactionTypeID
                FROM Transactions
                INNER JOIN Categories cat
                On cat.Id = Transactions.CategoryID
                WHERE Transactions.Id = @Id AND Transactions.UserID = @userid", new { id, userid });

        }

        public async Task Delete(int Id)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("Delete_Transactions", new { Id }, commandType: System.Data.CommandType.StoredProcedure);

        }
    }
}
