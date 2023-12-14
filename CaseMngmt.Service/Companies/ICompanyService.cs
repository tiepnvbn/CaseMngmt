using CaseMngmt.Models.Companies;

namespace CaseMngmt.Service.Companies
{
    public interface ICompanyService
    {
        Task<int> AddAsync(CompanyRequest Company);
        Task<IEnumerable<CompanyViewModel>> GetAllAsync(string name, string phoneNumber, int pageSize, int pageNumber);
        Task<CompanyViewModel> GetByIdAsync(Guid id);
        Task<int> DeleteAsync(Guid id);
        Task<int> UpdateAsync(Guid Id, CompanyRequest Company);
    }
}
