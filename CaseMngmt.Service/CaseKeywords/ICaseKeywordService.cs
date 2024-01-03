using CaseMngmt.Models.CaseKeywords;
using CaseMngmt.Models.FileUploads;

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
        Task<Guid?> AddFileToKeywordAsync(CaseKeywordFileUpload fileUploadRequest, string filePath, Guid templateId);
        Task<IEnumerable<FileResponse>> GetFileKeywordsByCaseIdAAsync(Guid caseId);
        Task<int> DeleteFileKeywordAsync(Guid caseId, Guid keywordId);
    }
}
