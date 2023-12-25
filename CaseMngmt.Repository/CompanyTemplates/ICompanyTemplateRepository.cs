using CaseMngmt.Models.CompanyTemplates;

namespace CaseMngmt.Repository.CompanyTemplates
{
    public interface ICompanyTemplateRepository
    {
        Task<int> AddMultiAsync(List<CompanyTemplate> request);
        Task<int> AddAsync(CompanyTemplate request);
        //Task<IEnumerable<CaseKeywordValue>> GetAllAsync(CaseKeywordSearchRequest searchRequest);
        //Task<IEnumerable<CaseKeywordBaseValue>> GetByIdAsync(Guid caseId);
        //Task<int> DeleteAsync(Guid caseId);
        //Task<int> DeleteByCaseIdAsync(Guid caseId);
        //Task<int> UpdateAsync(CaseKeyword caseKey);
        //Task<int> UpdateMultiAsync(Guid caseId, List<CaseKeyword> caseKeys);
    }
}
