using CaseMngmt.Models.Templates;

namespace CaseMngmt.Service.Templates
{
    public interface ITemplateService
    {
        Task<int> AddAsync(TemplateAddRequest template);
        Task<IEnumerable<TemplateViewModel>> GetAllAsync(Guid? companyId, int pageSize, int pageNumber);
        Task<TemplateViewModel?> GetByIdAsync(Guid id);
        Task<int> DeleteAsync(Guid id);
        Task<int> UpdateAsync(TemplateRequest template);
    }
}
