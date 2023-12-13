using CaseMngmt.Models.Data;
using CaseMngmt.Models.Customers;
using Microsoft.EntityFrameworkCore;

namespace CaseMngmt.Repository.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddCustomerAsync(Customer customer)
        {
            try
            {
                await _context.Customer.AddAsync(customer);
                var result = _context.SaveChanges();

                return result;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<bool> CheckCustomerExistsAsync(string customerName)
        {
            var customerCount = await (from customer in _context.Customer
                                       where customer.Name == customerName
                                       select customer).CountAsync();

            if (customerCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            try
            {
                Customer customer = await _context.Customer.FindAsync(id);
                if (customer != null)
                {
                    _context.Customer.Remove(customer);
                }
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync(string customerName, string phoneNumber, string companyId, int pageSize, int pageNumber)
        {
            var IQueryableCustomer = (from tempCustomer in _context.Customer select tempCustomer);
            
            if (!string.IsNullOrEmpty(companyId))
            {
                IQueryableCustomer = IQueryableCustomer.Where(m => m.CompanyId == Guid.Parse(companyId));
            }

            if (!string.IsNullOrEmpty(customerName))
            {
                IQueryableCustomer = IQueryableCustomer.Where(m => m.Name.Contains(customerName.Trim()));
            }
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                IQueryableCustomer = IQueryableCustomer.Where(m => m.PhoneNumber.Contains(phoneNumber.Trim()));
            }
            
            IQueryableCustomer = IQueryableCustomer.OrderBy(m => m.Name);
            var result = await IQueryableCustomer.Skip(pageNumber - 1).Take(pageSize).ToListAsync();

            return result;
        }

        public async Task<Customer> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Customer.FindAsync(id);
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
                    _context.Customer.Update(customer);
                    await _context.SaveChangesAsync();
                    return 1;
                }

                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
