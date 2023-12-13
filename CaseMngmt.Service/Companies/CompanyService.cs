using AutoMapper;
using CaseMngmt.Models.Companies;
using CaseMngmt.Repository.Companies;

namespace CaseMngmt.Service.Companies
{
    public class CompanyService : ICompanyService
    {
        private ICompanyRepository _repository;
        private readonly IMapper _mapper;
        public CompanyService(ICompanyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(CompanyRequest Company)
        {
            try
            {
                var entity = _mapper.Map<Company>(Company);
                entity.CreatedDate = DateTime.UtcNow;
                entity.UpdatedDate = DateTime.UtcNow;
                return await _repository.AddAsync(entity);
            }
            catch (Exception)
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
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<IEnumerable<CompanyViewModel>> GetAllAsync(string CompanyName, string phoneNumber, int pageSize, int pageNumber)
        {

            var CompanysFromRepository = await _repository.GetAllAsync(CompanyName, phoneNumber, pageSize, pageNumber);

            var result = _mapper.Map<List<CompanyViewModel>>(CompanysFromRepository);

            return result;
        }

        public async Task<CompanyViewModel> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                var result = _mapper.Map<CompanyViewModel>(entity);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<int> UpdateAsync(Guid Id, CompanyRequest Company)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(Id);
                if (entity == null)
                {
                    return 0;
                }

                entity.Name = Company.Name;
                entity.RoomNumber = Company.RoomNumber;
                entity.City = Company.City;
                entity.PhoneNumber = Company.PhoneNumber;
                entity.BuildingName = Company.BuildingName;
                entity.City = Company.City;
                entity.Note = Company.Note;
                entity.PostCode1 = Company.PostCode1;
                entity.PostCode2 = Company.PostCode2;
                entity.StateProvince = Company.StateProvince;
                entity.UpdatedDate = DateTime.UtcNow;
                await _repository.UpdateAsync(entity);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
