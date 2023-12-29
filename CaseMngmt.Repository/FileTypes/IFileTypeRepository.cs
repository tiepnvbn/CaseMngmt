using CaseMngmt.Models.FileTypes;

namespace CaseMngmt.Repository.FileTypes
{
    public interface IFileTypeRepository
    {
        Task<int> AddAsync(FileType fileType);
        Task<int> AddMultiAsync(List<FileType> fileTypes);
        Task<IEnumerable<FileType>?> GetAllAsync(int pageSize, int pageNumber);
        Task<FileType?> GetByIdAsync(Guid id);
        Task<int> DeleteAsync(Guid id);
        Task<int> UpdateAsync(FileType model);
        Task<int> DeleteByIdsAsync(List<Guid> fileIds);
    }
}
