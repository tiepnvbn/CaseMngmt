using AutoMapper;
using CaseMngmt.Models.FileTypes;
using CaseMngmt.Repository.FileTypes;

namespace CaseMngmt.Service.FileTypes
{
    public class FileTypeService : IFileTypeService
    {
        private IFileTypeRepository _repository;
        private readonly IMapper _mapper;
        public FileTypeService(IFileTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(FileTypeRequest request)
        {
            try
            {
                var entity = _mapper.Map<FileType>(request);
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

        public async Task<IEnumerable<FileTypeViewModel>?> GetAllAsync()
        {
            try
            {
                var typesFromRepository = await _repository.GetAllAsync();
                var result = _mapper.Map<List<FileTypeViewModel>>(typesFromRepository);

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<FileTypeViewModel?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                var result = _mapper.Map<FileTypeViewModel>(entity);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> UpdateAsync(Guid Id, FileTypeRequest request)
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
