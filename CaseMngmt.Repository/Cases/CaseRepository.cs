﻿using CaseMngmt.Models.Database;
using CaseMngmt.Models.Cases;
using Microsoft.EntityFrameworkCore;

namespace CaseMngmt.Repository.Cases
{
    public class CaseRepository : ICaseRepository
    {
        private ApplicationDbContext _context;

        public CaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Case model)
        {
            try
            {
                await _context.Case.AddAsync(model);
                var result = _context.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<Case?> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _context.Case.FindAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Case>> GetAllAsync(int pageSize, int pageNumber)
        {
            var IQueryableCase = (from tempCase in _context.Case select tempCase);
            IQueryableCase = IQueryableCase.OrderBy(m => m.Name);
            var result = await IQueryableCase.Skip(pageNumber - 1).Take(pageSize).ToListAsync();

            return result;
        }

        public async Task<int> UpdateAsync(Case caseModel)
        {
            try
            {
                if (caseModel != null)
                {
                    _context.Case.Update(caseModel);
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
                Case caseModel = await _context.Case.FindAsync(id);
                if (caseModel != null)
                {
                    caseModel.Deleted = true;
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
