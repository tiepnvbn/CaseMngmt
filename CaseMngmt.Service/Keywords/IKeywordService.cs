using CaseMngmt.Models.Keywords;

namespace CaseMngmt.Service.Keywords
{
    public interface IKeywordService
    {
        Task<int> AddAsync(KeywordRequest request);
        Task<IEnumerable<KeywordViewModel>?> GetAllAsync(int pageSize, int pageNumber);
        Task<KeywordViewModel?> GetByIdAsync(Guid id);
        Task<int> DeleteAsync(Guid id);
        Task<int> UpdateAsync(Guid Id, KeywordRequest request);
    }
}
