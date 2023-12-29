﻿using CaseMngmt.Models.Database;
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

        public async Task<Keyword?> GetByIdAsync(Guid id)
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

        public async Task<List<Keyword>?> GetByTemplateIdAsync(Guid templateId)
        {
            try
            {
                var IQueryableKeyword = (from tempKeyword in _context.Keyword select tempKeyword);
                IQueryableKeyword = IQueryableKeyword.Where(x => !x.Deleted && x.TemplateId == templateId).OrderBy(m => m.Name);
                var result = await IQueryableKeyword.ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<KeywordViewModel>?> GetAllAsync(int pageSize, int pageNumber)
        {
            try
            {
                var IQueryableKeyword = (from tempKeyword in _context.Keyword
                                         join tempType in _context.Type on tempKeyword.TypeId equals tempType.Id
                                         where !tempKeyword.Deleted 
                                         select new KeywordViewModel
                                         {
                                             KeywordName = tempKeyword.Name,
                                             UpdatedBy = tempKeyword.UpdatedBy,
                                             UpdatedDate = tempKeyword.UpdatedDate,
                                             CreatedBy = tempKeyword.CreatedBy,
                                             CreatedDate = tempKeyword.CreatedDate,
                                             KeywordId = tempKeyword.Id,
                                             IsRequired = tempKeyword.IsRequired,
                                             MaxLength = tempKeyword.MaxLength,
                                             Order = tempKeyword.Order,
                                             Searchable = tempKeyword.Searchable,
                                             DocumentSearchable = tempKeyword.DocumentSearchable,
                                             TypeId = tempKeyword.Type.Id,
                                             TypeName = tempKeyword.Type.Name,
                                             TypeValue = tempKeyword.Type.Value,
                                             Metadata = !string.IsNullOrEmpty(tempKeyword.Type.Metadata)
                                                ? tempKeyword.Type.Metadata.Split(',', StringSplitOptions.None).ToList()
                                                : new List<string>()
                                         });
                IQueryableKeyword = IQueryableKeyword.OrderBy(m => m.KeywordName);
                var result = await IQueryableKeyword.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
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
                Keyword? keywordModel = await _context.Keyword.FindAsync(id);
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

        public async Task<int> DeleteMultiByTemplateIdAsync(Guid templateId)
        {
            try
            {
                var IQueryableKeyword = (from tempKeyword in _context.Keyword select tempKeyword);
                IQueryableKeyword = IQueryableKeyword.Where(x => !x.Deleted && x.TemplateId == templateId).OrderBy(m => m.Name);
                var result = await IQueryableKeyword.ToListAsync();

                foreach (var item in result)
                {
                    item.Deleted = true;
                    _context.Keyword.Update(item);
                }
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> AddMultiAsync(List<Keyword> keywords)
        {
            try
            {
                await _context.Keyword.AddRangeAsync(keywords);
                var result = _context.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> UpdateMultiAsync(Guid templateId, List<Keyword> keywords)
        {
            try
            {
                var data = _context.Keyword.Where(a => !a.Deleted && a.TemplateId == templateId).ToList();
                foreach (var item in data)
                {
                    item.Deleted = true;
                    _context.Keyword.Update(item);
                }
                await _context.SaveChangesAsync();

                if (keywords != null)
                {
                    await _context.Keyword.AddRangeAsync(keywords);
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
