using CaseMngmt.Models.Database;
using Microsoft.EntityFrameworkCore;
using CaseMngmt.Models.TemplateKeywords;
using CaseMngmt.Repository.TemplateKeywords;

namespace CaseMngmt.Repository.Cases
{
    public class TemplateKeywordRepository : ITemplateKeywordRepository
    {
        private ApplicationDbContext _context;

        public TemplateKeywordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(TemplateKeyword templateKey)
        {
            try
            {
                await _context.TemplateKeyword.AddAsync(templateKey);
                var result = _context.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> AddMultiAsync(List<TemplateKeyword> templateKeys)
        {
            try
            {
                await _context.TemplateKeyword.AddRangeAsync(templateKeys);
                var result = _context.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<IEnumerable<TemplateKeywordValue>> GetByIdAsync(Guid id)
        {
            try
            {
                var IQueryable = (from templateKeyword in _context.TemplateKeyword
                                  join keyword in _context.Keyword on templateKeyword.KeywordId equals keyword.Id
                                  join type in _context.Type on keyword.TypeId equals type.Id
                                  where templateKeyword.TemplateId == id
                                  select new TemplateKeywordValue
                                  {
                                      KeywordId = templateKeyword.KeywordId,
                                      TypeId = type.Id,
                                      TypeName = type.Name,
                                      RoleId = templateKeyword.RoleId,
                                      Searchable = templateKeyword.Searchable
                                  });
                var result = await IQueryable.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<TemplateKeyword>> GetAllAsync(int pageSize, int pageNumber)
        {
            var IQueryableCase = (from tempCase in _context.TemplateKeyword select tempCase);
            var result = await IQueryableCase.Skip(pageNumber - 1).Take(pageSize).ToListAsync();

            return result;
        }

        public async Task<int> UpdateAsync(TemplateKeyword templateKey)
        {
            try
            {
                if (templateKey != null)
                {
                    _context.TemplateKeyword.Update(templateKey);
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
                TemplateKeyword model = await _context.TemplateKeyword.FindAsync(id);
                if (model != null)
                {
                    _context.TemplateKeyword.Remove(model);
                }
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> DeleteByTemplateIdAbdKeywordIdAsync(Guid caseId)
        {
            try
            {
                var data = _context.TemplateKeyword.Where(a => a.TemplateId == caseId).ToList();
                foreach (var item in data)
                {
                    item.Deleted = true;
                    _context.TemplateKeyword.Update(item);
                }
                await _context.SaveChangesAsync();

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> UpdateMultiAsync(Guid caseId, List<TemplateKeyword> templateKeys)
        {
            try
            {
                var data = _context.TemplateKeyword.Where(a => a.TemplateId == caseId).ToList();
                foreach (var item in data)
                {
                    item.Deleted = true;
                    _context.TemplateKeyword.Update(item);
                }
                await _context.SaveChangesAsync();

                if (templateKeys != null)
                {
                    await _context.TemplateKeyword.AddRangeAsync(templateKeys);
                    var result = _context.SaveChanges();
                    return 1;
                }

                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public Task<int> DeleteByTemplateIdAndKeywordIdAsync(Guid templateId, Guid keywordId)
        {
            throw new NotImplementedException();
        }
    }
}
