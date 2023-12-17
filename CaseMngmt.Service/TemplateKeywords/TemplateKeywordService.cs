using AutoMapper;
using CaseMngmt.Models.CaseKeywords;
using CaseMngmt.Models.TemplateKeywords;
using CaseMngmt.Models.Templates;
using CaseMngmt.Repository.Keywords;
using CaseMngmt.Repository.TemplateKeywords;
using CaseMngmt.Repository.Templates;

namespace CaseMngmt.Service.TemplateKeywords
{
    public class TemplateKeywordService : ITemplateKeywordService
    {
        private ITemplateRepository _templateRepository;
        private ITemplateKeywordRepository _templateKeywordRepository;
        private IKeywordRepository _keywordRepository;

        private readonly IMapper _mapper;
        public TemplateKeywordService(ITemplateRepository templateRepository, ITemplateKeywordRepository templateKeywordRepository, IKeywordRepository keywordRepository, IMapper mapper)
        {
            _templateRepository = templateRepository;
            _templateKeywordRepository = templateKeywordRepository;
            _keywordRepository = keywordRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TemplateKeywordViewModel>> GetAllAsync(TemplateKeywordSearchRequest searchRequest)
        {

            var customersFromRepository = await _templateKeywordRepository.GetAllAsync(searchRequest.PageSize ?? 25, searchRequest.PageNumber ?? 1);

            //var result = _mapper.Map<IEnumerable<CustomerViewModel>>(customersFromRepository);

            return null;
        }

        public async Task<TemplateKeywordViewModel?> GetByIdAsync(Guid templateId)
        {
            try
            {
                var caseKeywordValues = await _templateKeywordRepository.GetByIdAsync(templateId);
                if (caseKeywordValues != null)
                {
                    var result = new TemplateKeywordViewModel
                    {
                        TemplateId = templateId,
                        TemplateKeywordValues = caseKeywordValues.ToList()
                    };
                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> AddAsync(TemplateKeywordRequest request)
        {
            try
            {
                var templateModel = new Template
                {
                    Name = $"Case - {DateTime.UtcNow}"
                };
                var caseResult = await _templateRepository.AddAsync(templateModel);

                if (caseResult <= 0)
                {
                    return 0;
                }

                var templateKeywords = request.KeywordValues.Select(x => new TemplateKeyword
                {
                    TemplateId = templateModel.Id,
                    KeywordId = x.KeywordId,
                    Order = x.Order,
                    RoleId = x.RoleId,
                    Searchable = x.Searchable ?? false
                }).ToList();

                var emplateKeywordResult = await _templateKeywordRepository.AddMultiAsync(templateKeywords);

                return emplateKeywordResult;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> UpdateAsync(TemplateKeywordRequest request)
        {
            try
            {
                var entity = await _templateRepository.GetByIdAsync(request.TemplateId);
                if (entity == null)
                {
                    return 0;
                }

                var templateKeywords = request.KeywordValues.Select(x => new TemplateKeyword
                {
                    TemplateId = request.TemplateId,
                    KeywordId = x.KeywordId,
                    Order = x.Order,
                    RoleId = x.RoleId,
                    Searchable = x.Searchable ?? false
                }).ToList();

                var templateKeywordResult = await _templateKeywordRepository.UpdateMultiAsync(request.TemplateId, templateKeywords);

                return templateKeywordResult;
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
                await _templateRepository.DeleteAsync(caseId);
                // TODO
                //await _templateKeywordRepository.DeleteByTemplateIdAndKeywordIdAsync(caseId);

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
