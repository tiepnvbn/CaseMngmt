using CaseMngmt.Models;
using CaseMngmt.Models.Cases;

namespace CaseMngmt.Repository.Cases
{
    public interface ICaseRepository
    {
        Task<int> AddAsync(Case customer);
        Task<PagedResult<Case>?> GetAllAsync(int pageSize, int pageNumber);
        Task<Case?> GetByIdAsync(Guid id);
        Task<int> DeleteAsync(Guid id);
        Task<int> UpdateAsync(Case customer);
    }
}
