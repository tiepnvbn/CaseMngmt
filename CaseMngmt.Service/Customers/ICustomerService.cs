using CaseMngmt.Models.Customers;

namespace CaseMngmt.Service.Customers
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
