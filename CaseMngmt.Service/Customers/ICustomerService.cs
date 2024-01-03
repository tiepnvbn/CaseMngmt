using CaseMngmt.Models.Customers;

namespace CaseMngmt.Service.Customers
{
    public interface ICustomerService
    {
        Task<int> AddCustomerAsync(CustomerRequest customer);
        Task<Models.PagedResult<CustomerViewModel>?> GetAllCustomersAsync(string? customerName, string? phoneNumber, string companyId, int pageSize, int pageNumber);
        Task<CustomerViewModel?> GetByIdAsync(Guid id);
        Task<int> DeleteAsync(Guid id);
        Task<int> UpdateCustomerAsync(Guid Id, CustomerRequest customer);
    }
}
