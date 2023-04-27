using BudgetingApp.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetingApp.Services
{

    public interface IAccountsRepository
    {
        Task<IEnumerable<Account>> accountFind(int userid);
        Task Create(Account account);
        Task Delete(int Id);
        Task<Account> GetViaId(int id, int userid);
        Task Update(AccountCreation accountCreation);
    }
    public class AccountsRepository : IAccountsRepository
    {
        private readonly string connectionString;

        public AccountsRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Create(Account account) {

            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Account (Name, AccountTypeID, Balance, Description)
                                                        VALUES (@Name, @AccountTypeID, @Balance, @Description); SELECT SCOPE_IDENTITY();", account);

            account.Id = id;

        }

        public async Task<IEnumerable<Account>> accountFind (int userid) {

            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Account>(@" SELECT Account.Id, Account.Name, Account.Balance, tc.Name AS AccountType 
                                                            FROM Account
                                                            INNER JOIN AccountOrderType tc
                                                            ON tc.Id = Account.AccountTypeID
                                                            WHERE tc.UserID = @UserID
                                                            ORDER BY tc.OrderTransaction", new {userid });

        }

        public async Task<Account> GetViaId(int id, int userid) {

            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Account>(
                @"SELECT Account.Id, Account.Name, AccountTypeId, Balance, Description
                FROM Account
                INNER JOIN AccountOrderType tc
                ON tc.Id = Account.AccountTypeID
                WHERE tc.UserID = @UserID AND Account.Id = @Id", new { id, userid });
                

        }

        public async Task Update(AccountCreation accountCreation)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync(@"UPDATE Account SET Name = @Name, AccountTypeID = @AccountTypeID, Balance = @Balance, Description = @Description WHERE Id = @Id", accountCreation);

        }

        public async Task Delete(int Id)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("DELETE Account WHERE Id = @Id", new { Id });

        }
    }
}
