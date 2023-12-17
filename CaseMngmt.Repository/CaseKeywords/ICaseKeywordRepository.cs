using CaseMngmt.Models.CaseKeywords;

namespace CaseMngmt.Repository.Cases
{
    public interface ICaseKeywordRepository
    {
        Task<int> AddMultiAsync(List<CaseKeyword> caseKeys);
        Task<int> AddAsync(CaseKeyword caseKey);
        Task<IEnumerable<CaseKeyword>> GetAllAsync(int pageSize, int pageNumber);
        Task<IEnumerable<CaseKeywordValue>> GetByIdAsync(Guid id);
        Task<int> DeleteAsync(Guid id);
        Task<int> DeleteByCaseIdAsync(Guid caseId);
        Task<int> UpdateAsync(CaseKeyword caseKey);
        Task<int> UpdateMultiAsync(Guid caseId, List<CaseKeyword> caseKeys);
    }
}
