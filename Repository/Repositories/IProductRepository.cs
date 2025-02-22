using Domain.Entities;

namespace Repository.Repositories;

public interface IProductRepository
{
    Task AddAsync(Product product);
    void Update(Product product);
    bool Delete(int id, int deletedBy);
    IQueryable<Product> GetAll();
    Task<Product> GetByIdAsync(int id);
}
