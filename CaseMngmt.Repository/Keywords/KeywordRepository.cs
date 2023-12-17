using CaseMngmt.Models.Database;
using Microsoft.EntityFrameworkCore;
using CaseMngmt.Models.Keywords;

namespace CaseMngmt.Repository.Keywords
{
    public class KeywordRepository : IKeywordRepository
    {
        private ApplicationDbContext _context;

        public KeywordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Keyword model)
        {
            try
            {
                await _context.Keyword.AddAsync(model);
                var result = _context.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<Keyword> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _context.Keyword.FindAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Keyword>> GetAllAsync(int pageSize, int pageNumber)
        {
            var IQueryableKeyword = (from tempKeyword in _context.Keyword select tempKeyword);
            IQueryableKeyword = IQueryableKeyword.OrderBy(m => m.Name);
            var result = await IQueryableKeyword.Skip(pageNumber - 1).Take(pageSize).ToListAsync();

            return result;
        }

        public async Task<int> UpdateAsync(Keyword keywordModel)
        {
            try
            {
                if (keywordModel != null)
                {
                    _context.Keyword.Update(keywordModel);
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
                Keyword keywordModel = await _context.Keyword.FindAsync(id);
                if (keywordModel != null)
                {
                    keywordModel.Deleted = true;
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
    }
}
