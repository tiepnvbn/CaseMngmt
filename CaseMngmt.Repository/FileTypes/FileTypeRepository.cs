using CaseMngmt.Models.Database;
using CaseMngmt.Models.FileTypes;
using Microsoft.EntityFrameworkCore;

namespace CaseMngmt.Repository.FileTypes
{
    public class FileTypeRepository : IFileTypeRepository
    {
        private ApplicationDbContext _context;

        public FileTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(FileType model)
        {
            try
            {
                await _context.FileType.AddAsync(model);
                var result = _context.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<FileType?> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _context.FileType.FindAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<FileType>?> GetAllAsync(int pageSize, int pageNumber)
        {
            try
            {
                var IQueryableType = (from tempType in _context.FileType select tempType);
                IQueryableType = IQueryableType.Where(x => !x.Deleted).OrderBy(m => m.Name);
                var result = await IQueryableType.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> UpdateAsync(FileType model)
        {
            try
            {
                if (model != null)
                {
                    _context.FileType.Update(model);
                    await _context.SaveChangesAsync();
                    return 1;
                }

                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            try
            {
                FileType? typeModel = await _context.FileType.FindAsync(id);
                if (typeModel != null)
                {
                    typeModel.Deleted = true;
                    await _context.SaveChangesAsync();
                    return 1;
                }

                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> AddMultiAsync(List<FileType> fileTypes)
        {
            try
            {
                await _context.FileType.AddRangeAsync(fileTypes);
                var result = _context.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> DeleteByIdsAsync(List<Guid> fileIds)
        {
            try
            {
                var data = _context.FileType.Where(a => !a.Deleted && fileIds.Contains(a.Id)).ToList();
                foreach (var item in data)
                {
                    item.Deleted = true;
                    _context.FileType.Update(item);
                }
                await _context.SaveChangesAsync();

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
