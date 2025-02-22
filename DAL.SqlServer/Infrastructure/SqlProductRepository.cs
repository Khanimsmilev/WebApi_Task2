using DAL.SqlServer.Context;
using Dapper;
using Domain.Entities;
using Repository.Repositories;

namespace DAL.SqlServer.Infrastructure;

public class SqlProductRepository(string connectionString, AppDbContext context = null) : BaseSqlRepository(connectionString), IProductRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddAsync(Product product)
    {
        var sql = @"INSERT INTO Product([Name],[CreatedBy])
                    VALUES (@Name, @CreatedBy); SELECT SCOPE_IDENTITY()";

        using var conn = OpenConnection();
        var generatedId =await conn.ExecuteScalarAsync<int>(sql, product);

        //await conn.QueryAsync(sql, product);
    }

    public bool Delete(int id, int deletedBy)
    {
        var checkSql = @"SELECT Id FROM Products WHERE Id = @id AND IsDeleted=0";

        var sql = @"Update Products
            SET IsDeleted=1,
            DeletedBy =@deletedBy,
            DeletedDate = GETDATE(),
            WHERE Id = @id";
        using var conn = OpenConnection();
        using var transaction = conn.BeginTransaction();

        var productId = conn.ExecuteScalar<int?>(checkSql, id, transaction);

        if (!productId.HasValue)
        {
            return false;
        }

        var affectedRows = conn.Execute(sql, new { id, deletedBy }, transaction);
        transaction.Commit();
        return affectedRows > 0;
    }

    public IQueryable<Product> GetAll()
    {
        return _context.Products.OrderByDescending(c => c.CreatedDate).Where(c => c.IsDeleted == false);
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        var sql = @"SELECT P.*
        FROM Products AS P
        WHERE P.Id = @id AND P.IsDeleted = 0";

        using var conn = OpenConnection();
        return await conn.QueryFirstOrDefaultAsync<Product>(sql, id);
    }

    public void Update(Product product)
    {
        var sql = @"UPDATE Products
                   SET Name = @Name,
                   UpdatedBy =@UpdatedBy,
                   UpdatedDate = GETDATE()
                   WHERE Id = @Id";

        using var conn = OpenConnection();
        conn.Query(sql, product);
    }

}
