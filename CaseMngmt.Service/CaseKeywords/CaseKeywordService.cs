﻿using AutoMapper;
using CaseMngmt.Models.CaseKeywords;
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
        private ITypeRepository _typeRepository;
        private IKeywordRepository _keywordRepository;

        private readonly IMapper _mapper;
        public CaseKeywordService(ICaseRepository caseRepository, ICaseKeywordRepository caseKeywordRepository, ITypeRepository typeRepository, IKeywordRepository keywordRepository, IMapper mapper)
        {
            _caseRepository = caseRepository;
            _caseKeywordRepository = caseKeywordRepository;
            _typeRepository = typeRepository;
            _keywordRepository = keywordRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CaseKeywordViewModel>?> GetAllAsync(CaseKeywordSearchRequest searchRequest)
        {
            try
            {
                var result = await _caseKeywordRepository.GetAllAsync(searchRequest);
                if (result == null)
                {
                    return null;
                }

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
                    Name = $"Case - {DateTime.Now}",
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

        public async Task<int> AddFileToKeywordAsync(CaseKeywordFileUpload fileUploadRequest, Guid templateId)
        {
            try
            {
                var fileType = await _typeRepository.GetByTypeNameAsync("file");
                if (fileType == null)
                {
                    fileType = new Models.Types.Type()
                    {
                        Name = $"File",
                        Value = "file",
                        IsDefaultType = false,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    };
                    await _typeRepository.AddAsync(fileType);
                }

                var keyword = new Keyword()
                {
                    Name = "File",
                    TypeId = fileType.Id,
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
                    CaseId = fileUploadRequest.CaseId,
                    KeywordId = keyword.Id,
                    Value = fileUploadRequest.FileName
                };
                return await _caseKeywordRepository.AddAsync(caseKeyword);
            }
            catch (Exception ex)
            {
                return 0;
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
    }
}
