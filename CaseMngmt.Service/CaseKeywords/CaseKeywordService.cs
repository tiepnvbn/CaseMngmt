using AutoMapper;
using CaseMngmt.Models;
using CaseMngmt.Models.CaseKeywords;
using CaseMngmt.Models.FileUploads;
using CaseMngmt.Models.GenericValidation;
using CaseMngmt.Models.Keywords;
using CaseMngmt.Repository.CaseKeywords;
using CaseMngmt.Repository.Cases;
using CaseMngmt.Repository.Keywords;
using CaseMngmt.Repository.Types;
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

        public async Task<PagedResult<CaseKeywordViewModel>?> GetAllAsync(CaseKeywordSearchRequest searchRequest)
        {
            try
            {
                var result = await _caseKeywordRepository.GetAllAsync(searchRequest);

                return result;
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
                    CaseName = caseEntity.Name,
                    Status = caseEntity.Status,
                    CaseKeywordValues = caseKeywordValues.ToList()
                };
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Guid?> AddAsync(CaseKeywordAddRequest request)
        {
            try
            {
                var caseModel = new Models.Cases.Case
                {
                    Name = $"CASE{((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds()}",
                    Status = "Open"
                };
                var caseResult = await _caseRepository.AddAsync(caseModel);

                if (caseResult <= 0)
                {
                    return null;
                }

                var caseKeywords = request.KeywordValues.Select(x => new CaseKeyword
                {
                    CaseId = caseModel.Id,
                    KeywordId = x.KeywordId,
                    Value = x.Value
                }).ToList();

                var caseKeywordResult = await _caseKeywordRepository.AddMultiAsync(caseKeywords);
                if (caseKeywordResult > 0)
                {
                    return caseModel.Id;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
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

        public async Task<Guid?> AddFileToKeywordAsync(Guid caseId, Guid fileTypeId, FileUploadResponse fileResponse, Guid templateId)
        {
            try
            {
                var keyword = new Keyword()
                {
                    Name = fileResponse.FileName,
                    TypeId = fileTypeId,
                    TemplateId = templateId,
                    IsRequired = false,
                    Order = 0,
                    CaseSearchable = false,
                    DocumentSearchable = true,
                    IsShowOnCaseList = false,
                    IsShowOnTemplate = false
                };
                await _keywordRepository.AddAsync(keyword);

                var caseKeyword = new CaseKeyword
                {
                    CaseId = caseId,
                    KeywordId = keyword.Id,
                    Value = fileResponse.FilePath
                };

                var caseKeyResult = await _caseKeywordRepository.AddAsync(caseKeyword);
                return caseKeyResult > 0 ? keyword.Id : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> CloseCaseByAsync(Guid caseId)
        {
            try
            {
                var caseEntity = await _caseRepository.GetByIdAsync(caseId);
                if (caseEntity == null)
                {
                    return 0;
                }

                caseEntity.Status = "Closed";
                caseEntity.UpdatedDate = DateTime.Now;
                // TODO
                caseEntity.UpdatedBy = Guid.Empty;
                return await _caseRepository.UpdateAsync(caseEntity);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> DeleteFileKeywordAsync(Guid caseId, Guid keywordId)
        {
            try
            {
                var caseEntity = await _caseRepository.GetByIdAsync(caseId);
                if (caseEntity == null)
                {
                    return 0;
                }

                var keywordEntity = await _keywordRepository.GetByIdAsync(keywordId);
                if (keywordEntity == null)
                {
                    return 0;
                }
                var caseKeyword = await _caseKeywordRepository.GetByCaseIdAndKeywordIdAsync(caseId, keywordId);
                if (caseKeyword != null)
                {
                    await _caseKeywordRepository.DeleteAsync(caseKeyword.Id);
                    await _keywordRepository.DeleteAsync(keywordEntity.Id);
                }

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<IEnumerable<FileResponse>> GetFileKeywordsByCaseIdAsync(Guid caseId)
        {
            try
            {
                var fileKeywordsResult = await _caseKeywordRepository.GetFileKeywordsByCaseIdAsync(caseId);
                foreach (var item in fileKeywordsResult)
                {
                    string ext = Path.GetExtension(item.FileName).ToLower();
                    item.IsImage = DataTypeDictionary.ImageTypes.Contains(ext);
                }
                return fileKeywordsResult;
            }
            catch (Exception ex)
            {
                return new List<FileResponse>();
            }
        }

        public async Task<PagedResult<CaseKeywordViewModel>?> GetDocumentsAsync(DocumentSearchRequest searchRequest)
        {
            try
            {
                var result = await _caseKeywordRepository.GetDocumentsAsync(searchRequest);

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
