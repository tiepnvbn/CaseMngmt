using CaseMngmt.Models.Cases;

namespace CaseMngmt.Service.Cases
{
    public interface ICaseService
    {
        Task<int> AddAsync(string caseName);
        Task<IEnumerable<Case>?> GetAllAsync(int pageSize, int pageNumber);
        Task<Case?> GetByIdAsync(Guid id);
        Task<int> DeleteAsync(Guid id);
        Task<int> UpdateAsync(Guid Id, string caseName);
    }
}
