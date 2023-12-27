using CaseMngmt.Models.Database;
using CaseMngmt.Models.Companies;
using Microsoft.EntityFrameworkCore;

namespace CaseMngmt.Repository.Companies
{
    public class CompanyRepository : ICompanyRepository
    {
        private ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Company Company)
        {
            try
            {
                await _context.Company.AddAsync(Company);
                var result = _context.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            try
            {
                Company? company = await _context.Company.FindAsync(id);
                if (company != null)
                {
                    company.Deleted = true;
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

        public async Task<IEnumerable<Company>> GetAllAsync(string companyName, string phoneNumber, int pageSize, int pageNumber)
        {
            try
            {
                var IQueryableCompany = (from tempCompany in _context.Company select tempCompany).Where(x => !x.Deleted);

                if (!string.IsNullOrEmpty(companyName))
                {
                    IQueryableCompany = IQueryableCompany.Where(m => m.Name.Contains(companyName.Trim()));
                }
                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    IQueryableCompany = IQueryableCompany.Where(m => m.PhoneNumber.Contains(phoneNumber.Trim()));
                }

                IQueryableCompany = IQueryableCompany.OrderBy(m => m.Name);
                var result = await IQueryableCompany.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Company?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Company.FindAsync(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> UpdateAsync(Company company)
        {
            try
            {
                if (company != null)
                {
                    _context.Company.Update(company);
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
