using CaseMngmt.Models.Customers;

namespace CaseMngmt.Repository.Customers
{
    public interface ICustomerRepository
    {
        Task<int> AddAsync(Customer customer);
        Task<bool> CheckCustomerExistsAsync(string customerName);
        Task<IEnumerable<Customer>> GetAllAsync(string customerName, string phoneNumber, string companyId, int pageSize, int pageNumber);
        Task<Customer> GetByIdAsync(Guid id);
        Task<int> DeleteAsync(Guid id);
        Task<int> UpdateAsync(Customer customer);
    }
}
