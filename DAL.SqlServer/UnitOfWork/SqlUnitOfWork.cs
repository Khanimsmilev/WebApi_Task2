using DAL.SqlServer.Context;
using DAL.SqlServer.Infrastructure;
using Repository.Common;
using Repository.Repositories;

namespace DAL.SqlServer.UnitOfWork;

public class SqlUnitOfWork(string connectionString, AppDbContext context) : IUnitOfWork
{
    private readonly string _connectionString = connectionString;
    private readonly AppDbContext _context = context;

    public SqlCategoryRepository _categoryRepository;
    public SqlCustomerRepository _customerRepository;
    public SqlProductRepository _productRepository;

    public ICategoryRepository CategoryRepository => _categoryRepository ?? new SqlCategoryRepository(_connectionString, _context);

    public ICustomerRepository CustomerRepository => _customerRepository ?? new SqlCustomerRepository(_connectionString, _context);

    public IProductRepository ProductRepository => _productRepository ?? new SqlProductRepository(_connectionString, _context);

}
