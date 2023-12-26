using CaseMngmt.Models.Database;
using Microsoft.EntityFrameworkCore;
using CaseMngmt.Models.CaseKeywords;
using CaseMngmt.Models.CompanyTemplates;

namespace CaseMngmt.Repository.CompanyTemplates
{
    public class CompanyTemplateRepository : ICompanyTemplateRepository
    {
        private ApplicationDbContext _context;

        public CompanyTemplateRepository(ApplicationDbContext context)
        {
            _context = context;
        }
       
        public async Task<int> AddAsync(CompanyTemplate request)
        {
            try
            {
                await _context.CompanyTemplate.AddAsync(request);
                var result = _context.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> AddMultiAsync(List<CompanyTemplate> request)
        {
            try
            {
                await _context.CompanyTemplate.AddRangeAsync(request);
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
                                      KeywordName = keyword.Name,
                                      Value = caseKeyword.Value,
                                      IsRequired = keyword.IsRequired,
                                      MaxLength = keyword.MaxLength,
                                      Searchable = keyword.Searchable,
                                      Order = keyword.Order,
                                      TypeId = type.Id,
                                      TypeName = type.Name,
                                      Metadata = !string.IsNullOrEmpty(keyword.Metadata)
                                                ? keyword.Metadata.Split(',', StringSplitOptions.None).ToList()
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

        public async Task<IEnumerable<CaseKeywordValue>> GetAllAsync(CaseKeywordSearchRequest searchRequest)
        {
            try
            {
                var IQueryableCase = (from caseKeyword in _context.CaseKeyword
                                      join keyword in _context.Keyword on caseKeyword.KeywordId equals keyword.Id
                                      join template in _context.Template on keyword.TemplateId equals template.Id
                                      join companyTemplate in _context.CompanyTemplate on template.Id equals companyTemplate.TemplateId
                                      join type in _context.Type on keyword.TypeId equals type.Id
                                      where !caseKeyword.Deleted
                                        && keyword.TemplateId == searchRequest.TemplateId
                                        && companyTemplate.CompanyId == searchRequest.CompanyId
                                      select new CaseKeywordValue
                                      {
                                          CaseId = caseKeyword.CaseId,
                                          KeywordId = caseKeyword.KeywordId,
                                          KeywordName = keyword.Name,
                                          Value = caseKeyword.Value,
                                          IsRequired = keyword.IsRequired,
                                          MaxLength = keyword.MaxLength,
                                          Searchable = keyword.Searchable,
                                          Order = keyword.Order,
                                          TypeId = type.Id,
                                          TypeName = type.Name,
                                          Metadata = !string.IsNullOrEmpty(keyword.Metadata)
                                                ? keyword.Metadata.Split(',', StringSplitOptions.None).ToList()
                                                : new List<string>()
                                      });
                var result = await IQueryableCase.Skip(searchRequest.PageNumber - 1).Take(searchRequest.PageSize).ToListAsync();

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

        public async Task<List<CompanyTemplate>> GetTemplateByCompanyIdAsync(Guid companyId)
        {
            try
            {
                List<CompanyTemplate> companyTemplateawait = await _context.CompanyTemplate.Where(x => x.CompanyId.Equals(companyId)).ToListAsync();
                return companyTemplateawait;
            }
            catch (Exception)
            {
                return new List<CompanyTemplate>();
                throw;
            }
        }
    }
}
