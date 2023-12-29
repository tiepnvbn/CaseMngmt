using Type = CaseMngmt.Models.Types.Type;

namespace CaseMngmt.Repository.Types
{
    public interface ITypeRepository
    {
        Task<int> AddAsync(Type Type);
        Task<int> AddMultiAsync(List<Type> Types);
        Task<IEnumerable<Type>?> GetAllAsync(int pageSize, int pageNumber);
        Task<Type?> GetByIdAsync(Guid id);
        Task<int> DeleteAsync(Guid id);
        Task<int> UpdateAsync(Type Type);
        Task<int> DeleteByIdsAsync(List<Guid> typeIds);
    }
}
