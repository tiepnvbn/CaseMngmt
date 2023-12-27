using AutoMapper;
using CaseMngmt.Models.CaseKeywords;
using CaseMngmt.Models.Cases;
using CaseMngmt.Repository.CaseKeywords;
using CaseMngmt.Repository.Cases;
using CaseMngmt.Repository.Keywords;
using CaseMngmt.Service.CaseKeywords;

namespace CaseMngmt.Service.Customers
{
    public class CaseKeywordService : ICaseKeywordService
    {
        private ICaseRepository _caseRepository;
        private ICaseKeywordRepository _caseKeywordRepository;
        private IKeywordRepository _keywordRepository;

        private readonly IMapper _mapper;
        public CaseKeywordService(ICaseRepository caseRepository, ICaseKeywordRepository caseKeywordRepository, IKeywordRepository keywordRepository, IMapper mapper)
        {
            _caseRepository = caseRepository;
            _caseKeywordRepository = caseKeywordRepository;
            _keywordRepository = keywordRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CaseKeywordViewModel>?> GetAllAsync(CaseKeywordSearchRequest searchRequest)
        {
            try
            {
                var caseKeywordValues = await _caseKeywordRepository.GetAllAsync(searchRequest);
                if (caseKeywordValues == null)
                {
                    return null;
                }

                //var result = caseKeywordValues.Select(x => 
                //    new CaseKeywordViewModel
                //    {
                //        //CaseId = caseId,
                //        CaseKeywordValues = x.ToList()
                //    }
                //);

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<CaseKeywordViewModel?> GetByIdAsync(Guid caseId)
        {
            try
            {
                var caseEntity = await _caseRepository.GetByIdAsync(caseId);
                if (caseEntity == null)
                {
                    return null;
                }
                var caseKeywordValues = await _caseKeywordRepository.GetByIdAsync(caseId);
                if (caseKeywordValues == null)
                {
                    return null;
                }

                var result = new CaseKeywordViewModel
                {
                    CaseId = caseId,
                    CaseKeywordValues = caseKeywordValues.ToList()
                };
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> AddAsync(CaseKeywordAddRequest request)
        {
            try
            {
                var caseModel = new Models.Cases.Case
                {
                    Name = $"Case - {DateTime.UtcNow}"
                };
                var caseResult = await _caseRepository.AddAsync(caseModel);

                if (caseResult <= 0)
                {
                    return 0;
                }

                var caseKeywords = request.KeywordValues.Select(x => new CaseKeyword
                {
                    CaseId = caseModel.Id,
                    KeywordId = x.KeywordId,
                    Value = x.Value
                }).ToList();

                var caseKeywordResult = await _caseKeywordRepository.AddMultiAsync(caseKeywords);

                return caseKeywordResult;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> UpdateAsync(CaseKeywordRequest request)
        {
            try
            {
                var entity = await _caseRepository.GetByIdAsync(request.CaseId);
                if (entity == null)
                {
                    return 0;
                }

                var caseKeywords = request.KeywordValues.Select(x => new CaseKeyword
                {
                    CaseId = request.CaseId,
                    KeywordId = x.KeywordId,
                    Value = x.Value
                }).ToList();

                var caseKeywordResult = await _caseKeywordRepository.UpdateMultiAsync(request.CaseId, caseKeywords);

                return caseKeywordResult;
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
                await _caseKeywordRepository.DeleteByCaseIdAsync(caseId);
                await _caseRepository.DeleteAsync(caseId);

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
