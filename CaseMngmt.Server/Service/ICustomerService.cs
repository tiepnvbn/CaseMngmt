using CaseMngmt.Server.Models.Customers;

namespace CaseMngmt.Server.Service
{
    public interface ICustomerService
    {
        Task<int> AddCustomerAsync(Customer customer);
        Task<bool> CheckCustomerExistsAsync(string customerName);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer> GetByIdAsync(int id);
        Task<int> DeleteAsync(int id);
        Task<int> UpdateCustomerAsync(Customer customer);
    }
}
