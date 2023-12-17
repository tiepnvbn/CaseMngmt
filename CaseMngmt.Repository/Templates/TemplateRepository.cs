using CaseMngmt.Models.Database;
using Microsoft.EntityFrameworkCore;
using CaseMngmt.Models.Templates;

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

        public async Task<IEnumerable<Template>> GetAllAsync(int pageSize, int pageNumber)
        {
            var IQueryableTemplate = (from tempTemplate in _context.Template select tempTemplate);
            IQueryableTemplate = IQueryableTemplate.OrderBy(m => m.Name);
            var result = await IQueryableTemplate.Skip(pageNumber - 1).Take(pageSize).ToListAsync();

            return result;
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
    }
}
