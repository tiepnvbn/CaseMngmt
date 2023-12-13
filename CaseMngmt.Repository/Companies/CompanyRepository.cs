using CaseMngmt.Models.Data;
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
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            try
            {
                Company Company = await _context.Company.FindAsync(id);
                if (Company != null)
                {
                    _context.Company.Remove(Company);
                }
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<IEnumerable<Company>> GetAllAsync(string CompanyName, string phoneNumber, int pageSize, int pageNumber)
        {
            var IQueryableCompany = (from tempCompany in _context.Company select tempCompany);

            if (!string.IsNullOrEmpty(CompanyName))
            {
                IQueryableCompany = IQueryableCompany.Where(m => m.Name.Contains(CompanyName.Trim()));
            }
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                IQueryableCompany = IQueryableCompany.Where(m => m.PhoneNumber.Contains(phoneNumber.Trim()));
            }
            
            IQueryableCompany = IQueryableCompany.OrderBy(m => m.Name);
            var result = await IQueryableCompany.Skip(pageNumber - 1).Take(pageSize).ToListAsync();

            return result;
        }

        public async Task<Company> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Company.FindAsync(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<int> UpdateAsync(Company Company)
        {
            try
            {
                if (Company != null)
                {
                    _context.Company.Update(Company);
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
