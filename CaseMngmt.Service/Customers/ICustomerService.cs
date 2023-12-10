using CaseMngmt.Models.Customers;

namespace CaseMngmt.Service.Customers
{
    public interface ICustomerService
    {
        Task<int> AddCustomerAsync(CustomerRequest customer);
        Task<bool> CheckCustomerExistsAsync(string customerName);
        Task<IEnumerable<CustomerViewModel>> GetAllCustomersAsync(string customerName, string phoneNumber, int pageSize, int pageNumber);
        Task<CustomerViewModel> GetByIdAsync(Guid id);
        Task<int> DeleteAsync(Guid id);
        Task<int> UpdateCustomerAsync(Guid Id, CustomerRequest customer);
    }
}
