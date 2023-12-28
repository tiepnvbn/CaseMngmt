using CaseMngmt.Models.Database;
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

        public async Task<int> AddAsync(Customer customer)
        {
            try
            {
                await _context.Customer.AddAsync(customer);
                var result = _context.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<bool> CheckCustomerExistsAsync(string customerName)
        {
            var customerCount = await (from customer in _context.Customer
                                       where !customer.Deleted && customer.Name == customerName
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
                Customer? customer = await _context.Customer.FindAsync(id);
                if (customer != null)
                {
                    customer.Deleted = true;
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

        public async Task<IEnumerable<Customer>> GetAllAsync(string customerName, string phoneNumber, string companyId, int pageSize, int pageNumber)
        {
            try
            {
                var IQueryableCustomer = (from tempCustomer in _context.Customer select tempCustomer).Where(x => !x.Deleted);

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
                var result = await IQueryableCustomer.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Customer.FindAsync(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> UpdateAsync(Customer customer)
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
