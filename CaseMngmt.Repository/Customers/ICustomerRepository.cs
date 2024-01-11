using CaseMngmt.Models;
using CaseMngmt.Models.Customers;

namespace CaseMngmt.Repository.Customers
{
    public interface ICustomerRepository
    {
        Task<int> AddAsync(Customer customer);
        Task<PagedResult<Customer>?> GetAllAsync(string? customerName, string? phoneNumber, string companyId, int pageSize, int pageNumber);
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(Guid id);
        Task<int> DeleteAsync(Guid id);
        Task<int> UpdateAsync(Customer customer);
    }
}
