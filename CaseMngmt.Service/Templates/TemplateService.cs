using AutoMapper;
using CaseMngmt.Models.Companies;
using CaseMngmt.Models.Templates;
using CaseMngmt.Repository.Companies;
using CaseMngmt.Repository.Templates;

namespace CaseMngmt.Service.Templates
{
    public class TemplateService : ITemplateService
    {
        private ITemplateRepository _repository;
        private readonly IMapper _mapper;
        public TemplateService(ITemplateRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(TemplateRequest request)
        {
            try
            {
                var entity = _mapper.Map<Template>(request);
                entity.CreatedDate = DateTime.UtcNow;
                entity.UpdatedDate = DateTime.UtcNow;
                return await _repository.AddAsync(entity);
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
                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<IEnumerable<TemplateViewModel>> GetAllAsync(int pageSize, int pageNumber)
        {
            var templatesFromRepository = await _repository.GetAllAsync(pageSize, pageNumber);

            var result = _mapper.Map<List<TemplateViewModel>>(templatesFromRepository);

            return result;
        }

        public async Task<TemplateViewModel> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                var result = _mapper.Map<TemplateViewModel>(entity);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> UpdateAsync(Guid Id, TemplateRequest request)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(Id);
                if (entity == null)
                {
                    return 0;
                }

                entity.Name = request.Name;
                entity.UpdatedDate = DateTime.UtcNow;
                await _repository.UpdateAsync(entity);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
