using CaseMngmt.Models.TemplateKeywords;

namespace CaseMngmt.Service.TemplateKeywords
{
    public interface ITemplateKeywordService
    {
        Task<int> AddAsync(TemplateKeywordRequest request);
        Task<IEnumerable<TemplateKeywordViewModel>> GetAllAsync(TemplateKeywordSearchRequest searchRequest);
        Task<TemplateKeywordViewModel> GetByIdAsync(Guid caseId);
        Task<int> DeleteAsync(Guid caseId);
        Task<int> UpdateAsync(TemplateKeywordRequest request);
    }
}
