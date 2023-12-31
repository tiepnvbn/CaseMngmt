using CaseMngmt.Models.CaseKeywords;

namespace CaseMngmt.Service.CaseKeywords
{
    public interface ICaseKeywordService
    {
        Task<Guid?> AddAsync(CaseKeywordAddRequest request);
        Task<IEnumerable<CaseKeywordViewModel>?> GetAllAsync(CaseKeywordSearchRequest searchRequest);
        Task<CaseKeywordViewModel?> GetByIdAsync(Guid caseId);
        Task<int> CloseCaseByAsync(Guid caseId);
        Task<int> DeleteAsync(Guid caseId);
        Task<int> UpdateAsync(CaseKeywordRequest request);
        Task<int> AddFileToKeywordAsync(CaseKeywordFileUpload fileUploadRequest, Guid templateId);
    }
}
