using CaseMngmt.Models.TemplateKeywords;

namespace CaseMngmt.Repository.TemplateKeywords
{
    public interface ITemplateKeywordRepository
    {
        Task<int> AddMultiAsync(List<TemplateKeyword> templateKeys);
        Task<int> AddAsync(TemplateKeyword templateKey);
        Task<IEnumerable<TemplateKeyword>> GetAllAsync(int pageSize, int pageNumber);
        Task<IEnumerable<TemplateKeywordValue>> GetByIdAsync(Guid templateId);
        Task<int> DeleteAsync(Guid templateId);
        Task<int> DeleteByTemplateIdAndKeywordIdAsync(Guid templateId, Guid keywordId);
        Task<int> UpdateAsync(TemplateKeyword templateKey);
        Task<int> UpdateMultiAsync(Guid templateId, List<TemplateKeyword> templateKeys);
    }
}
