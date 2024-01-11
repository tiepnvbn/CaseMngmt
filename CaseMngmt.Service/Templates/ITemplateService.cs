using CaseMngmt.Models.CaseKeywords;
using CaseMngmt.Models.Templates;

namespace CaseMngmt.Service.Templates
{
    public interface ITemplateService
    {
        Task<int> AddAsync(TemplateRequest template);
        Task<Models.PagedResult<TemplateViewModel>?> GetAllAsync(Guid? companyId, int pageSize, int pageNumber);
        Task<TemplateViewModel?> GetByIdAsync(Guid id, bool isGetCustomer = false);
        Task<CaseTemplate?> GetCaseSearchModelByIdAsync(Guid templateId, List<Guid> roleIds);
        Task<DocumentTemplateResponse?> GetDocumentSearchModelByIdAsync(Guid templateId);
        Task<int> DeleteAsync(Guid id);
        Task<int> UpdateAsync(TemplateViewRequest template);
    }
}
