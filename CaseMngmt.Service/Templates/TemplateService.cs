using AutoMapper;
using CaseMngmt.Models.Templates;
using CaseMngmt.Repository.Keywords;
using CaseMngmt.Repository.Templates;
using CaseMngmt.Models.Keywords;
using CaseMngmt.Repository.Types;

namespace CaseMngmt.Service.Templates
{
    public class TemplateService : ITemplateService
    {
        private ITemplateRepository _repository;
        private IKeywordRepository _keywordRepository;
        private ITypeRepository _typeRepository;
        private readonly IMapper _mapper;
        public TemplateService(ITemplateRepository repository, IKeywordRepository keywordRepository, ITypeRepository typeRepository, IMapper mapper)
        {
            _repository = repository;
            _keywordRepository = keywordRepository;
            _typeRepository = typeRepository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(TemplateRequest request)
        {
            try
            {
                Template template = new Template()
                {
                    Name = $"Template {DateTime.Today}"
                };

                var response = await _repository.AddAsync(template);
                if (response <= 0)
                {
                    return 0;
                }

                var typeList = (await _typeRepository.GetAllAsync(25, 1))?.FirstOrDefault(x => x.IsDefaultType && x.Value == "list");
                if (typeList == null)
                {
                    return 0;
                }

                var typeListEntities = new List<Models.Types.Type>();
                var keywordEntities = new List<Keyword>();

                var typeListRequest = request.KeywordRequests.Where(x => x.TypeId == typeList.Id);
                if (typeListRequest != null && typeListRequest.Any())
                {
                    typeListEntities = typeListRequest.Select(x => new Models.Types.Type()
                    {
                        Name = $"{x.Name} - List Metadata",
                        Source = x.Source,
                        Metadata = x.Metadata,
                        Value = "list",
                        IsDefaultType = false
                    }).ToList();
                    await _typeRepository.AddMultiAsync(typeListEntities);
                }

                for (var i = 0; i < request.KeywordRequests.Count; i++)
                {
                    var item = request.KeywordRequests[i];
                    var newListId = typeListEntities.FirstOrDefault(z => z.Name == $"{item.Name} - List Metadata")?.Id ?? Guid.Empty;
                    keywordEntities.Add(new Keyword()
                    {
                        Name = item.Name,
                        TypeId = item.TypeId == typeList.Id ? newListId : item.TypeId,
                        TemplateId = template.Id,
                        IsRequired = item.IsRequired,
                        MaxLength = item.MaxLength,
                        Order = item.Order,
                        CaseSearchable = item.CaseSearchable,
                        DocumentSearchable = item.DocumentSearchable,
                        IsShowOnCaseList = item.IsShowOnCaseList,
                        IsShowOnTemplate = true
                    });
                }

                response = await _keywordRepository.AddMultiAsync(keywordEntities);
                return response;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> UpdateAsync(TemplateViewRequest request)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(request.TemplateId);
                if (entity == null)
                {
                    return 0;
                }

                var currentKeywords = await _keywordRepository.GetByTemplateIdAsync(request.TemplateId);
                if (currentKeywords != null && currentKeywords.Any())
                {
                    await _keywordRepository.DeleteMultiByTemplateIdAsync(request.TemplateId);
                }

                // Check new Type LIST
                var typeList = (await _typeRepository.GetAllAsync(25, 1))?.FirstOrDefault(x => x.IsDefaultType && x.Value == "list");
                if (typeList == null)
                {
                    return 0;
                }

                var typeListEntities = new List<Models.Types.Type>();
                var keywordEntities = new List<Keyword>();

                var typeListRequest = request.KeywordRequests.Where(x => x.TypeId == typeList.Id);
                if (typeListRequest != null && typeListRequest.Any())
                {
                    typeListEntities = typeListRequest.Select(x => new Models.Types.Type()
                    {
                        Name = $"{x.Name} - List Metadata",
                        Source = x.Source,
                        Metadata = x.Metadata,
                        Value = "list",
                        IsDefaultType = false
                    }).ToList();
                    await _typeRepository.AddMultiAsync(typeListEntities);
                }

                for (var i = 0; i < request.KeywordRequests.Count; i++)
                {
                    var item = request.KeywordRequests[i];
                    var newListId = typeListEntities.FirstOrDefault(z => z.Name == $"{item.Name} - List Metadata")?.Id ?? Guid.Empty;
                    keywordEntities.Add(new Keyword()
                    {
                        Name = item.Name,
                        TypeId = item.TypeId == typeList.Id ? newListId : item.TypeId,
                        TemplateId = request.TemplateId,
                        IsRequired = item.IsRequired,
                        MaxLength = item.MaxLength,
                        Order = item.Order,
                        CaseSearchable = item.CaseSearchable,
                        DocumentSearchable = item.DocumentSearchable,
                        IsShowOnCaseList = item.IsShowOnCaseList,
                        IsShowOnTemplate = true
                    });
                }

                await _keywordRepository.AddMultiAsync(keywordEntities);

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> DeleteAsync(Guid templateId)
        {
            try
            {
                var currentTemplate = await _repository.GetTemplateViewModelByIdAsync(templateId);
                if (currentTemplate == null)
                {
                    return 0;
                }

                var currentKeywords = await _keywordRepository.GetByTemplateIdAsync(templateId);
                if (currentKeywords != null && currentKeywords.Any())
                {
                    await _keywordRepository.DeleteMultiByTemplateIdAsync(templateId);
                }

                await _repository.DeleteAsync(templateId);

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<Models.PagedResult<TemplateViewModel>?> GetAllAsync(Guid? companyId, int pageSize, int pageNumber)
        {
            try
            {
                var result = await _repository.GetAllAsync(companyId, pageSize, pageNumber);

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<TemplateViewModel?> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _repository.GetTemplateViewModelByIdAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
