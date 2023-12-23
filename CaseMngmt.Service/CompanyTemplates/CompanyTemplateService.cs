//using AutoMapper;
//using CaseMngmt.Models.CompanyTemplates;
//using CaseMngmt.Repository.Companies;

//namespace CaseMngmt.Service.CompanyTemplates
//{
//    public class CompanyTemplateService : ICompanyTemplateService
//    {
//        private ICompanyRepository _repository;
//        private readonly IMapper _mapper;
//        public CompanyTemplateService(ICompanyRepository repository, IMapper mapper)
//        {
//            _repository = repository;
//            _mapper = mapper;
//        }

//        public async Task<int> AddAsync(CompanyTemplate Company)
//        {
//            try
//            {
//                return await _repository.AddAsync(entity);
//            }
//            catch (Exception ex)
//            {
//                return 0;
//            }
//        }

//        public async Task<int> DeleteAsync(Guid id)
//        {
//            try
//            {
//                return await _repository.DeleteAsync(id);
//            }
//            catch (Exception ex)
//            {
//                return 0;
//            }
//        }

//        public async Task<CompanyTemplate> GetByIdAsync(Guid id)
//        {
//            try
//            {
//                var entity = await _repository.GetByIdAsync(id);
//                return result;
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        public async Task<int> UpdateAsync(CompanyTemplate request)
//        {
//            try
//            {
//                var entity = await _repository.GetByIdAsync(Id);
//                if (entity == null)
//                {
//                    return 0;
//                }

//                entity.UpdatedDate = DateTime.UtcNow;
//                await _repository.UpdateAsync(entity);
//                return 1;
//            }
//            catch (Exception ex)
//            {
//                return 0;
//            }
//        }
//    }
//}
