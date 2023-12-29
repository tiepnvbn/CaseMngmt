using CaseMngmt.Models.Types;

namespace CaseMngmt.Service.Types
{
    public interface ITypeService
    {
        Task<int> AddAsync(TypeRequest request);
        Task<IEnumerable<TypeViewModel>?> GetAllAsync(int pageSize, int pageNumber);
        Task<TypeViewModel?> GetByIdAsync(Guid id);
        Task<int> DeleteAsync(Guid id);
        Task<int> UpdateAsync(Guid Id, TypeRequest request);
    }
}
