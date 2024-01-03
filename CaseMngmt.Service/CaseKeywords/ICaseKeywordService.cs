using CaseMngmt.Models;
using CaseMngmt.Models.CaseKeywords;
using CaseMngmt.Models.FileUploads;

namespace CaseMngmt.Service.CaseKeywords
{
    public interface ICaseKeywordService
    {
        Task<Guid?> AddAsync(CaseKeywordAddRequest request);
        Task<PagedResult<CaseKeywordViewModel>?> GetAllAsync(CaseKeywordSearchRequest searchRequest);
        Task<CaseKeywordViewModel?> GetByIdAsync(Guid caseId);
        Task<int> CloseCaseByAsync(Guid caseId);
        Task<int> DeleteAsync(Guid caseId);
        Task<int> UpdateAsync(CaseKeywordRequest request);
        Task<Guid?> AddFileToKeywordAsync(CaseKeywordFileUpload fileUploadRequest, string filePath, Guid templateId);
        Task<IEnumerable<FileResponse>> GetFileKeywordsByCaseIdAsync(Guid caseId);
        Task<int> DeleteFileKeywordAsync(Guid caseId, Guid keywordId);
    }
}
