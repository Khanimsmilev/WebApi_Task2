using DAL.SqlServer.Context;
using Dapper;
using Domain.Entities;
using Repository.Repositories;

namespace DAL.SqlServer.Infrastructure;

public class SqlCustomerRepository(string connectionString, AppDbContext context = null) : BaseSqlRepository(connectionString), ICustomerRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddAsync(Customer customer)
    {
        var sql = @"INSERT INTO Customers([Name],[CreatedBy])
                    VALUES (@Name, @CreatedBy); SELECT SCOPE_IDENTITY()";

        using var conn = OpenConnection();
        var generatedId = await conn.ExecuteScalarAsync<int>(sql, customer);
        customer.Id = generatedId;

        //await conn.QueryAsync(sql, customer);
    }

    public bool Delete(int id, int deletedBy)
    {
        var checkSql = @"SELECT Id FROM Customers WHERE Id = @id AND IsDeleted=0";

        var sql = @"Update Customers
            SET IsDeleted=1,
            DeletedBy =@deletedBy,
            DeletedDate = GETDATE(),
            WHERE Id = @id";
        using var conn = OpenConnection();
        using var transaction = conn.BeginTransaction();

        var customerId = conn.ExecuteScalar<int?>(checkSql, id, transaction);

        if (!customerId.HasValue)
        {
            return false;
        }

        var affectedRows = conn.Execute(sql, new { id, deletedBy }, transaction);
        transaction.Commit();
        return affectedRows > 0;
    }

    public IQueryable<Customer> GetAll()
    {
        return _context.Customers.OrderByDescending(c => c.CreatedDate).Where(c => c.IsDeleted == false);
    }

    public async Task<Customer> GetByIdAsync(int id)
    {
        var sql = @"SELECT C.*
        FROM Customers AS C
        WHERE C.Id = @id AND C.IsDeleted = 0";

        using var conn = OpenConnection();
        return await conn.QueryFirstOrDefaultAsync<Customer>(sql, id);
    }

    public void Update(Customer customer)
    {
        var sql = @"UPDATE Customers
                   SET Name = @Name,
                   UpdatedBy =@UpdatedBy,
                   UpdatedDate = GETDATE()
                   WHERE Id = @Id";

        using var conn = OpenConnection();
        conn.Query(sql, customer);
    }

}
