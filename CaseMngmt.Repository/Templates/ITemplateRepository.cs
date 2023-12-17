using CaseMngmt.Models.Templates;

namespace CaseMngmt.Repository.Templates
{
    public interface ITemplateRepository
    {
        Task<int> AddAsync(Template customer);
        Task<IEnumerable<Template>> GetAllAsync(int pageSize, int pageNumber);
        Task<Template> GetByIdAsync(Guid id);
        Task<int> DeleteAsync(Guid id);
        Task<int> UpdateAsync(Template customer);
    }
}
