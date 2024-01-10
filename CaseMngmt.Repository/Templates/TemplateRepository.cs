using CaseMngmt.Models.Database;
using Microsoft.EntityFrameworkCore;
using CaseMngmt.Models.Templates;
using CaseMngmt.Models;
using CaseMngmt.Models.Keywords;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace CaseMngmt.Repository.Templates
{
    public class TemplateRepository : ITemplateRepository
    {
        private ApplicationDbContext _context;

        public TemplateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Template template)
        {
            try
            {
                await _context.Template.AddAsync(template);
                var result = _context.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<Template?> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _context.Template.FindAsync(id);

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<TemplateViewModel?> GetTemplateViewModelByIdAsync(Guid templateId)
        {
            try
            {
                var IQueryableTemplate = (from tempTemplate in _context.Template.Include(x => x.Keywords)
                                          join keyword in _context.Keyword on tempTemplate.Id equals keyword.TemplateId
                                          join type in _context.Type on keyword.TypeId equals type.Id
                                          where !tempTemplate.Deleted && tempTemplate.Id == templateId
                                          select new TemplateViewModel
                                          {
                                              Id = templateId,
                                              CreatedBy = tempTemplate.CreatedBy,
                                              CreatedDate = tempTemplate.CreatedDate,
                                              Name = tempTemplate.Name,
                                              UpdatedBy = tempTemplate.UpdatedBy,
                                              UpdatedDate = tempTemplate.UpdatedDate,
                                              Keywords = tempTemplate.Keywords
                                                .Where(x => x.IsShowOnTemplate)
                                                .Select(x => new KeywordViewModel
                                                {
                                                    KeywordName = x.Name,
                                                    UpdatedBy = x.UpdatedBy,
                                                    UpdatedDate = x.UpdatedDate,
                                                    CreatedBy = x.CreatedBy,
                                                    CreatedDate = x.CreatedDate,
                                                    KeywordId = x.Id,
                                                    IsRequired = x.IsRequired,
                                                    MaxLength = x.MaxLength,
                                                    Order = x.Order,
                                                    Searchable = x.CaseSearchable,
                                                    DocumentSearchable = x.DocumentSearchable,
                                                    IsShowOnTemplate = x.IsShowOnTemplate,
                                                    IsShowOnCaseList = x.IsShowOnCaseList,
                                                    TemplateId = templateId,
                                                    TypeId = x.Type.Id,
                                                    TypeName = x.Type.Name,
                                                    TypeValue = x.Type.Value,
                                                    Metadata = !string.IsNullOrEmpty(x.Type.Metadata)
                                                    ? x.Type.Metadata.Split(',', StringSplitOptions.None).ToList()
                                                    : new List<string>()
                                                }).OrderBy(x => x.Order).ToList()
                                          });

                var result = await IQueryableTemplate.FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<PagedResult<TemplateViewModel>?> GetAllAsync(Guid? companyId, int pageSize, int pageNumber)
        {
            try
            {
                var queryableTemplate = (from tempTemplate in _context.Template
                                         join companyTemplate in _context.CompanyTemplate on tempTemplate.Id equals companyTemplate.TemplateId
                                         join keyword in _context.Keyword on tempTemplate.Id equals keyword.TemplateId
                                         join type in _context.Type on keyword.TypeId equals type.Id
                                         where !tempTemplate.Deleted
                                         select new TemplateViewModel
                                         {
                                             Id = tempTemplate.Id,
                                             Name = tempTemplate.Name,
                                             CreatedBy = tempTemplate.CreatedBy,
                                             CreatedDate = tempTemplate.CreatedDate,
                                             UpdatedBy = tempTemplate.UpdatedBy,
                                             UpdatedDate = tempTemplate.UpdatedDate,
                                             Keywords = tempTemplate.Keywords.Select(x => new Models.Keywords.KeywordViewModel
                                             {
                                                 KeywordName = x.Name,
                                                 UpdatedBy = x.UpdatedBy,
                                                 UpdatedDate = x.UpdatedDate,
                                                 CreatedBy = x.CreatedBy,
                                                 CreatedDate = x.CreatedDate,
                                                 KeywordId = x.Id,
                                                 IsRequired = x.IsRequired,
                                                 MaxLength = x.MaxLength,
                                                 Order = x.Order,
                                                 Searchable = x.CaseSearchable,
                                                 DocumentSearchable = x.DocumentSearchable,
                                                 IsShowOnTemplate = x.IsShowOnTemplate,
                                                 IsShowOnCaseList = x.IsShowOnCaseList,
                                                 TemplateId = tempTemplate.Id,
                                                 TypeId = x.Type.Id,
                                                 TypeName = x.Type.Name,
                                                 TypeValue = x.Type.Value,
                                             }).ToList()
                                         });

                //// TODO if have time
                //if (companyId != Guid.Empty)
                //{
                //    IQueryableTemplate = IQueryableTemplate.Where(x => x).OrderBy(m => m.Name);
                //}

                queryableTemplate = queryableTemplate.OrderBy(m => m.Name);
                var query = queryableTemplate.AsQueryable();
                var result = await PagedResult<TemplateViewModel>.CreateAsync(query.AsNoTracking(), pageNumber, pageSize);

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> UpdateAsync(Template template)
        {
            try
            {
                if (template != null)
                {
                    _context.Template.Update(template);
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
                Template? template = await _context.Template.FindAsync(id);
                if (template != null)
                {
                    template.Deleted = true;
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

        public async Task<List<KeywordSearchModel>> GetCaseSearchModelByIdAsync(Guid templateId)
        {
            try
            {
                var IQueryableTemplate = (from tempTemplate in _context.Template.Include(x => x.Keywords)
                                          join keyword in _context.Keyword on tempTemplate.Id equals keyword.TemplateId
                                          join type in _context.Type on keyword.TypeId equals type.Id
                                          where !tempTemplate.Deleted
                                            && tempTemplate.Id == templateId
                                            && keyword.CaseSearchable
                                          select new KeywordSearchModel
                                          {
                                              KeywordName = keyword.Name,
                                              KeywordId = keyword.Id,
                                              MaxLength = keyword.MaxLength,
                                              Order = keyword.Order,
                                              TypeId = keyword.Type.Id,
                                              TypeName = keyword.Type.Name,
                                              TypeValue = keyword.Type.Value,
                                              Metadata = !string.IsNullOrEmpty(keyword.Type.Metadata)
                                                    ? keyword.Type.Metadata.Split(',', StringSplitOptions.None).ToList()
                                                    : new List<string>()
                                          }).OrderBy(x => x.Order);

                var result = await IQueryableTemplate.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                return new List<KeywordSearchModel>();
            }
        }

        public async Task<List<KeywordSearchModel>> GetDocumentSearchModelByIdAsync(Guid templateId)
        {
            try
            {
                var IQueryableTemplate = (from tempTemplate in _context.Template.Include(x => x.Keywords)
                                          join keyword in _context.Keyword on tempTemplate.Id equals keyword.TemplateId
                                          join type in _context.Type on keyword.TypeId equals type.Id
                                          where !tempTemplate.Deleted
                                            && tempTemplate.Id == templateId
                                            && keyword.DocumentSearchable
                                          select new KeywordSearchModel
                                          {
                                              KeywordName = keyword.Name,
                                              KeywordId = keyword.Id,
                                              MaxLength = keyword.MaxLength,
                                              Order = keyword.Order,
                                              TypeId = keyword.Type.Id,
                                              TypeName = keyword.Type.Name,
                                              TypeValue = keyword.Type.Value,
                                              Metadata = !string.IsNullOrEmpty(keyword.Type.Metadata)
                                                    ? keyword.Type.Metadata.Split(',', StringSplitOptions.None).ToList()
                                                    : new List<string>()
                                          }).OrderBy(x => x.Order);

                var result = await IQueryableTemplate.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                return new List<KeywordSearchModel>();
            }
        }
    }
}
