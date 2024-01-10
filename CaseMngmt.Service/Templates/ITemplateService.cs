using CaseMngmt.Models.Keywords;
using CaseMngmt.Models.Templates;

namespace CaseMngmt.Service.Templates
{
    public interface ITemplateService
    {
        Task<int> AddAsync(TemplateRequest template);
        Task<Models.PagedResult<TemplateViewModel>?> GetAllAsync(Guid? companyId, int pageSize, int pageNumber);
        Task<TemplateViewModel?> GetByIdAsync(Guid id);
        Task<List<KeywordSearchModel>> GetCaseSearchModelByIdAsync(Guid id);
        Task<List<KeywordSearchModel>> GetDocumentSearchModelByIdAsync(Guid id);
        Task<int> DeleteAsync(Guid id);
        Task<int> UpdateAsync(TemplateViewRequest template);
    }
}
