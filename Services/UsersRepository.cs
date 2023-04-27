using BudgetingApp.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Net.NetworkInformation;

namespace BudgetingApp.Services
{

    public interface IUsersRepository
    {
        Task<int> CreateUser(User user);
        Task<User> FindUserviaEmail(string emailNormalized);
    }
    public class UsersRepository: IUsersRepository
    {

        private readonly string connectionString;

        public UsersRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CreateUser(User user)
        {
            using var connection = new SqlConnection(connectionString);
            var userid = await connection.QuerySingleAsync<int>(@"INSERT INTO Users (Email, EmailNormalized, PasswordHash)
                                                        VALUES (@Email, @EmailNormalized, @PasswordHash); SELECT SCOPE_IDENTITY();", user);

            await connection.ExecuteAsync("CreateDataNewUser", new { userid }, commandType: System.Data.CommandType.StoredProcedure);
            return userid;

        }

        public async Task<User> FindUserviaEmail(string emailNormalized)
        {

            using var connection = new SqlConnection(connectionString);
            return await connection.QuerySingleOrDefaultAsync<User>(@"SELECT * FROM Users Where EmailNormalized = @emailNormalized", new { emailNormalized });

        }

    }
}

        

