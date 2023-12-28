﻿using CaseMngmt.Models.CaseKeywords;

namespace CaseMngmt.Repository.CaseKeywords
{
    public interface ICaseKeywordRepository
    {
        Task<int> AddMultiAsync(List<CaseKeyword> caseKeys);
        Task<int> AddAsync(CaseKeyword caseKey);
        Task<List<CaseKeywordViewModel>> GetAllAsync(CaseKeywordSearchRequest searchRequest);
        Task<IEnumerable<CaseKeywordBaseValue>> GetByIdAsync(Guid caseId);
        Task<int> DeleteAsync(Guid caseId);
        Task<int> DeleteByCaseIdAsync(Guid caseId);
        Task<int> UpdateAsync(CaseKeyword caseKey);
        Task<int> UpdateMultiAsync(Guid caseId, List<CaseKeyword> caseKeys);
    }
}
