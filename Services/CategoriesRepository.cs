using BudgetingApp.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BudgetingApp.Services
{

    public interface ICategoryRepository
    {
        Task Create(Category category);
        Task Delete(int Id);
        Task<IEnumerable<Category>> GetCategory(int UserID);
        Task<IEnumerable<Category>> GetOperationType(int UserID, OperationType operationType);
        Task<Category> GetViaId(int id, int userid);
        Task Update(Category category);
    }
    public class CategoriesRepository: ICategoryRepository
    {
        private readonly string connectionString;
        public CategoriesRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");

        }


        public async Task Create(Category category)
        {

            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Categories (Name, TransactionTypeID, UserId)
                                                        VALUES (@Name, @OperationTypeID, @UserId); SELECT SCOPE_IDENTITY();", category);

            category.Id = id;

        }

        public async Task<IEnumerable<Category>> GetCategory(int UserID)
        {

            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<Category>(@"SELECT Id, Name, TransactionTypeId AS OperationTypeID, UserId FROM Categories WHERE UserID = @UserID",
                                                                 new { UserID });

        }

        public async Task<IEnumerable<Category>> GetOperationType(int UserID, OperationType operationType)
        {

            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<Category>(@"SELECT Id, Name, TransactionTypeId AS OperationTypeID, UserId FROM Categories WHERE UserID = @UserID AND
                                                          TransactionTypeId = @operationType",
                                                                 new { UserID, operationType });

        }
        public async Task<Category> GetViaId(int Id, int userid)
        {

            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Category>(
            @"SELECT Id, Name, TransactionTypeId AS OperationTypeID, UserID FROM Categories WHERE Id = @Id AND UserID = @UserID", new { Id, userid });
            //@"SELECT * FROM Categories WHERE Id = @Id AND UserID = @UserID", new { id, userid });

        }

        public async Task Update(Category category)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync(@"UPDATE Categories SET Name = @Name, TransactionTypeID = @OperationTypeID WHERE Id = @Id", category);
        }

        public async Task Delete(int Id)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("DELETE Categories WHERE Id = @Id", new { Id });

        }
    }
}
