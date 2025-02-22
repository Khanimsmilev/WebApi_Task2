using Domain.Entities;

namespace Repository.Repositories;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer);
    void Update(Customer customer);
    bool Delete(int id, int deletedBy);
    IQueryable<Customer> GetAll();
    Task<Customer> GetByIdAsync(int id);
}
