using CaseMngmt.Models.Companies;
using CaseMngmt.Models.Templates;

namespace CaseMngmt.Service.Templates
{
    public interface ITemplateService
    {
        Task<int> AddAsync(TemplateRequest template);
        Task<IEnumerable<TemplateViewModel>> GetAllAsync(int pageSize, int pageNumber);
        Task<TemplateViewModel> GetByIdAsync(Guid id);
        Task<int> DeleteAsync(Guid id);
        Task<int> UpdateAsync(Guid Id, TemplateRequest template);
    }
}
