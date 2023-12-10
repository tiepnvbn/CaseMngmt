using CaseMngmt.Models.Customers;

namespace CaseMngmt.Repository.Customers
{
    public interface ICustomerRepository
    {
        Task<int> AddCustomerAsync(Customer customer);
        Task<bool> CheckCustomerExistsAsync(string customerName);
        Task<IEnumerable<Customer>> GetAllCustomersAsync(string customerName, string phoneNumber, int pageSize, int pageNumber);
        Task<Customer> GetByIdAsync(int id);
        Task<int> DeleteAsync(int id);
        Task<int> UpdateCustomerAsync(Customer customer);
    }
}
