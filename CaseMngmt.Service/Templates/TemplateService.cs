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

                var types = request.KeywordRequests.Where(x => !string.IsNullOrEmpty(x.Metadata)).Select(x => new Models.Types.Type()
                {
                    Name = $"{x.TypeName} - {x.Name}",
                    Value = x.Metadata
                }).ToList();
                await _typeRepository.AddMultiAsync(types);

                var keywordEntities = request.KeywordRequests.Select(x => new Keyword()
                {
                    Name = x.Name,
                    TypeId = types.FirstOrDefault(z => z.Name == $"{x.TypeName} - {x.Name}").Id,
                    TemplateId = template.Id,
                    IsRequired = x.IsRequired,
                    MaxLength = x.MaxLength,
                    Order = x.Order,
                    Searchable = x.Searchable,
                }).ToList();
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
                    await _typeRepository.DeleteByIdsAsync(currentKeywords.Select(x => x.TypeId).ToList());
                }

                var types = request.KeywordRequests.Where(x => !string.IsNullOrEmpty(x.Metadata)).Select(x => new Models.Types.Type()
                {
                    Name = $"{x.TypeName} - {x.Name}",
                    Value = x.Metadata
                }).ToList();
                await _typeRepository.AddMultiAsync(types);

                var keywordEntities = request.KeywordRequests.Select(x => new Keyword()
                {
                    Name = x.Name,
                    TypeId = types.FirstOrDefault(z => z.Name == $"{x.TypeName} - {x.Name}").Id,
                    TemplateId = request.TemplateId,
                    IsRequired = x.IsRequired,
                    MaxLength = x.MaxLength,
                    Order = x.Order,
                    Searchable = x.Searchable,
                }).ToList();
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
                await _keywordRepository.DeleteMultiByTemplateIdAsync(templateId);

                if (currentKeywords != null && currentKeywords.Any())
                {
                    await _typeRepository.DeleteByIdsAsync(currentKeywords.Select(x => x.TypeId).ToList());
                }

                await _repository.DeleteAsync(templateId);

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<IEnumerable<TemplateViewModel>> GetAllAsync(Guid? companyId, int pageSize, int pageNumber)
        {
            var result = await _repository.GetAllAsync(companyId, pageSize, pageNumber);

            return result;
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
