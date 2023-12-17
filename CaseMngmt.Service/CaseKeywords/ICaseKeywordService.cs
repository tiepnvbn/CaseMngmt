using CaseMngmt.Models.CaseKeywords;

namespace CaseMngmt.Service.CaseKeywords
{
    public interface ICaseKeywordService
    {
        Task<int> AddAsync(CaseKeywordRequest request);
        Task<IEnumerable<CaseKeywordViewModel>> GetAllAsync(CaseKeywordSearchRequest searchRequest);
        Task<CaseKeywordViewModel> GetByIdAsync(Guid caseId);
        Task<int> DeleteAsync(Guid caseId);
        Task<int> UpdateAsync(CaseKeywordRequest request);
    }
}
