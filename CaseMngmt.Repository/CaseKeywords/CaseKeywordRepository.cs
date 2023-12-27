using CaseMngmt.Models.Database;
using Microsoft.EntityFrameworkCore;
using CaseMngmt.Models.CaseKeywords;
using CaseMngmt.Models.Templates;

namespace CaseMngmt.Repository.CaseKeywords
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
                                  select new CaseKeywordBaseValue
                                  {
                                      KeywordId = caseKeyword.KeywordId,
                                      KeywordName = caseKeyword.Keyword.Name,
                                      Value = caseKeyword.Value,
                                      IsRequired = caseKeyword.Keyword.IsRequired,
                                      MaxLength = caseKeyword.Keyword.MaxLength,
                                      Searchable = caseKeyword.Keyword.Searchable,
                                      Order = keyword.Order,
                                      TypeId = caseKeyword.Keyword.Type.Id,
                                      TypeName = caseKeyword.Keyword.Type.Name,
                                      TypeValue = caseKeyword.Keyword.Type.Value,
                                      Metadata = !string.IsNullOrEmpty(caseKeyword.Keyword.Metadata)
                                                ? caseKeyword.Keyword.Metadata.Split(',', StringSplitOptions.None).ToList()
                                                : new List<string>()
                                  });
                var result = await IQueryable.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<CaseKeywordViewModel>> GetAllAsync(CaseKeywordSearchRequest searchRequest)
        {
            try
            {
                //var groupingQuery = 
                //        from responses in _context.CaseKeyword
                //        group responses by new { responses.CaseId, responses.Value } into responseGroup
                //        select new 
                //        {
                //            CaseId = responseGroup.Key,
                //            KeywordValue = responseGroup.
                //        };
                var IQueryableCase = (from tempCase in _context.Case
                                      join caseKeyword in _context.CaseKeyword on tempCase.Id equals caseKeyword.CaseId
                                      join keyword in _context.Keyword on caseKeyword.KeywordId equals keyword.Id
                                      join template in _context.Template on keyword.TemplateId equals template.Id
                                      join companyTemplate in _context.CompanyTemplate on template.Id equals companyTemplate.TemplateId
                                      join type in _context.Type on keyword.TypeId equals type.Id
                                      where !caseKeyword.Deleted
                                        && keyword.TemplateId == searchRequest.TemplateId
                                        && companyTemplate.CompanyId == searchRequest.CompanyId
                                        //&& searchRequest.KeywordValues.All(x => x.KeywordId == keyword.Id && caseKeyword.Value.Contains(x.Value))
                                      //group new { tempCase, caseKeyword } by new { tempCase.Id } into pg
                                      //let firstCase = pg.FirstOrDefault()
                                      select new CaseKeywordViewModel
                                      {
                                          CaseId = tempCase.Id,
                                          CaseKeywordValues = tempCase.Keywords.Select(x => new CaseKeywordBaseValue
                                          {
                                              KeywordId = x.Id,
                                              KeywordName = x.Name,
                                              Value = caseKeyword.Value,
                                              IsRequired = x.IsRequired,
                                              MaxLength = x.MaxLength,
                                              Searchable = x.Searchable,
                                              Order = x.Order,
                                              TypeId = x.Type.Id,
                                              TypeName = x.Type.Name,
                                              TypeValue = x.Type.Value,
                                              Metadata = !string.IsNullOrEmpty(x.Metadata)
                                                ? x.Metadata.Split(',', StringSplitOptions.None).ToList()
                                                : new List<string>()
                                          }).ToList()
                                      }
                                      );
                var result = await IQueryableCase.Skip((searchRequest.PageNumber - 1) * searchRequest.PageSize).Take(searchRequest.PageSize).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
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
