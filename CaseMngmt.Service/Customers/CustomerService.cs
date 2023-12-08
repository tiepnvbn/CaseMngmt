using CaseMngmt.Models.Customers;
using CaseMngmt.Repository.Customers;

namespace CaseMngmt.Service.Customers
{
    public class CustomerService : ICustomerService
    {
        private ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> AddCustomerAsync(Customer customer)
        {
            try
            {
                return await _repository.AddCustomerAsync(customer);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<bool> CheckCustomerExistsAsync(string customerName)
        {
            return await _repository.CheckCustomerExistsAsync(customerName);
        }

        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _repository.GetAllCustomersAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<int> UpdateCustomerAsync(Customer customer)
        {
            try
            {
                if (customer != null)
                {
                    await _repository.UpdateCustomerAsync(customer);
                    return 1;
                }

                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
