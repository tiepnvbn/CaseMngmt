using CaseMngmt.Models.FileTypes;

namespace CaseMngmt.Service.FileTypes
{
    public interface IFileTypeService
    {
        Task<int> AddAsync(FileTypeRequest request);
        Task<IEnumerable<FileTypeViewModel>?> GetAllAsync(int pageSize, int pageNumber);
        Task<FileTypeViewModel?> GetByIdAsync(Guid id);
        Task<int> DeleteAsync(Guid id);
        Task<int> UpdateAsync(Guid Id, FileTypeRequest request);
    }
}
