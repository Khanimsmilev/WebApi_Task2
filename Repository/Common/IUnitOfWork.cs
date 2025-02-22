using Repository.Repositories;

namespace Repository.Common;

public interface IUnitOfWork
{
    public ICategoryRepository CategoryRepository { get; }
    public ICustomerRepository CustomerRepository { get; }
    public IProductRepository ProductRepository { get; }
}
