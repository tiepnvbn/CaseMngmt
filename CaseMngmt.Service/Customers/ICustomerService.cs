using CaseMngmt.Models.Customers;

namespace CaseMngmt.Service.Customers
{
    public interface ICustomerService
    {
        Task<int> AddCustomerAsync(CustomerViewModel customer);
        Task<bool> CheckCustomerExistsAsync(string customerName);
        Task<IEnumerable<CustomerViewModel>> GetAllCustomersAsync(string customerName, string phoneNumber, int pageSize, int pageNumber);
        Task<CustomerViewModel> GetByIdAsync(int id);
        Task<int> DeleteAsync(int id);
        Task<int> UpdateCustomerAsync(CustomerViewModel customer);
    }
}
