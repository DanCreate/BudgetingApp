using BudgetingApp.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetingApp.Services
{
    public interface IAccountTypeRepository
    {
        Task Create(AccountOrderType accountOrderType);
        Task Delete(int Id);
        Task<bool> Exists(string name, int userid);
        Task<IEnumerable<AccountOrderType>> GetAccount(int userid);
        Task<AccountOrderType> GetUserViaId(int Id, int UserID);
        Task Order(IEnumerable<AccountOrderType> accountTypeOrdered);
        Task Update(AccountOrderType accountOrderType);
    }
    public class AccountTypeRepository: IAccountTypeRepository
    {
        private readonly string connectionString;
        public AccountTypeRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");

        }
        public async Task Create(AccountOrderType accountOrderType) { 

            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO AccountOrderType (Name, UserID, OrderTransaction)
                                                               VALUES (@Name, @UserID, (SELECT COALESCE(MAX(OrderTransaction), 0) + 1 FROM 
                                                               AccountOrderType WHERE UserID = @UserID)); SELECT SCOPE_IDENTITY();", accountOrderType);

            accountOrderType.Id = id;
        
        }

        public async Task<bool> Exists(string Name, int UserID) {

            using var connection = new SqlConnection(connectionString);
            var exists = await connection.QueryFirstOrDefaultAsync<int>($"SELECT 1 FROM AccountOrderType WHERE Name = @Name AND UserID = @UserID;",
                                                                        new { Name, UserID });

            return exists == 1;
        }

        public async Task<IEnumerable<AccountOrderType>> GetAccount(int UserID)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<AccountOrderType>(@"SELECT Id, Name, OrderTransaction FROM AccountOrderType WHERE UserID = @UserID ORDER
                                                                 BY OrderTransaction",
                                                                 new { UserID });
           
        }

        public async Task Update(AccountOrderType accountOrderType)
        {
            using var connection = new SqlConnection(connectionString);

             await connection.ExecuteAsync(@"UPDATE AccountOrderType SET Name = @Name WHERE Id = @Id", accountOrderType);

        }

        public async Task<AccountOrderType> GetUserViaId(int Id, int UserID)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<AccountOrderType>(@"SELECT Id, Name, OrderTransaction FROM AccountOrderType 
                                                                                WHERE Id = @Id AND UserID = @UserID", new { Id, UserID });

            

        }

        public async Task Delete(int Id)
        {
            using var connection = new SqlConnection(connectionString);

             await connection.ExecuteAsync("DELETE AccountOrderType WHERE Id = @Id", new { Id });

        }

        public async Task Order (IEnumerable<AccountOrderType> accountTypeOrdered)
        {
            var query = "UPDATE AccountOrderType SET OrderTransaction = @OrderTransaction Where Id = @Id";
            
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync(query, accountTypeOrdered);

        }

    }
}
