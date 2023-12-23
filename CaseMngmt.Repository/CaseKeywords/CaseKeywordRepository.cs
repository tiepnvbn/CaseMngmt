using CaseMngmt.Models.Database;
using Microsoft.EntityFrameworkCore;
using CaseMngmt.Models.CaseKeywords;

namespace CaseMngmt.Repository.Cases
{
    public class CaseKeywordRepository : ICaseKeywordRepository
    {
        private ApplicationDbContext _context;

        public CaseKeywordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(CaseKeyword caseKey)
        {
            try
            {
                await _context.CaseKeyword.AddAsync(caseKey);
                var result = _context.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> AddMultiAsync(List<CaseKeyword> caseKeys)
        {
            try
            {
                await _context.CaseKeyword.AddRangeAsync(caseKeys);
                var result = _context.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<IEnumerable<CaseKeywordBaseValue>> GetByIdAsync(Guid caseId)
        {
            try
            {
                var IQueryable = (from caseKeyword in _context.CaseKeyword
                                  join keyword in _context.Keyword on caseKeyword.KeywordId equals keyword.Id
                                  join type in _context.Type on keyword.TypeId equals type.Id
                                  where !caseKeyword.Deleted && caseKeyword.CaseId == caseId
                                  select new CaseKeywordValue
                                  {
                                      KeywordId = caseKeyword.KeywordId,
                                      KeywordName = keyword.Name,
                                      Value = caseKeyword.Value,
                                      IsRequired = keyword.IsRequired,
                                      MaxLength = keyword.MaxLength,
                                      Searchable = keyword.Searchable,
                                      Order = keyword.Order,
                                      TypeId = type.Id,
                                      TypeName = type.Name,

                                  });
                var result = await IQueryable.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<CaseKeyword>> GetAllAsync(CaseKeywordSearchRequest searchRequest)
        {
            //var IQueryableCase = (from caseKeyword in _context.CaseKeyword 
            //                      join keyword in _context.Keyword on caseKeyword.KeywordId equals keyword.Id
            //                      join type in _context.Type on keyword.TypeId equals type.Id
            //                      where !caseKeyword.Deleted && caseKeyword.CaseId == searchRequest.TemplateId
            //                      select new CaseKeywordValue
            //                      {
            //                          KeywordId = caseKeyword.KeywordId,
                                        //KeywordName = keyword.Name,
                                        //  Value = caseKeyword.Value,
                                        //  IsRequired = keyword.IsRequired,
                                        //  MaxLength = keyword.MaxLength,
                                        //  Searchable = keyword.Searchable,
                                        //  Order = keyword.Order,
                                        //  TypeId = type.Id,
                                        //  TypeName = type.Name
            //                      });
            //var result = await IQueryableCase.Skip(searchRequest.PageNumber.Value - 1).Take(searchRequest.PageSize.Value).ToListAsync();

            //return result;
            return null;
        }

        public async Task<int> UpdateAsync(CaseKeyword caseKey)
        {
            try
            {
                if (caseKey != null)
                {
                    _context.CaseKeyword.Update(caseKey);
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

        public async Task<int> DeleteAsync(Guid caseId)
        {
            try
            {
                CaseKeyword? model = await _context.CaseKeyword.FindAsync(caseId);
                if (model != null)
                {
                    _context.CaseKeyword.Remove(model);
                }
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> DeleteByCaseIdAsync(Guid caseId)
        {
            try
            {
                var data = _context.CaseKeyword.Where(a => !a.Deleted && a.CaseId == caseId).ToList();
                foreach (var item in data)
                {
                    item.Deleted = true;
                    _context.CaseKeyword.Update(item);
                }
                await _context.SaveChangesAsync();

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> UpdateMultiAsync(Guid caseId, List<CaseKeyword> caseKeys)
        {
            try
            {
                var data = _context.CaseKeyword.Where(a => !a.Deleted && a.CaseId == caseId).ToList();
                foreach (var item in data)
                {
                    item.Deleted = true;
                    _context.CaseKeyword.Update(item);
                }
                await _context.SaveChangesAsync();

                if (caseKeys != null)
                {
                    await _context.CaseKeyword.AddRangeAsync(caseKeys);
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
    }
}
