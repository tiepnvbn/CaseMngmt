using CaseMngmt.Models.Keywords;

namespace CaseMngmt.Repository.Keywords
{
    public interface IKeywordRepository
    {
        Task<int> AddAsync(Keyword keyword);
        Task<IEnumerable<Keyword>> GetAllAsync(int pageSize, int pageNumber);
        Task<Keyword> GetByIdAsync(Guid id);
        Task<int> DeleteAsync(Guid id);
        Task<int> UpdateAsync(Keyword keyword);
    }
}
