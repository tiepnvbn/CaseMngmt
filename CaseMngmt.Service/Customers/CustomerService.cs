using AutoMapper;
using CaseMngmt.Models.Customers;
using CaseMngmt.Repository.Customers;

namespace CaseMngmt.Service.Customers
{
    public class CustomerService : ICustomerService
    {
        private ICustomerRepository _repository;
        private readonly IMapper _mapper;
        public CustomerService(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddCustomerAsync(CustomerRequest customer)
        {
            try
            {
                var entity = _mapper.Map<Customer>(customer);
                entity.CompanyId = customer.CompanyId;
                entity.CreatedDate = DateTime.UtcNow;
                entity.CreatedBy = Guid.Parse(customer.CreatedBy);
                entity.UpdatedDate = DateTime.UtcNow;
                entity.UpdatedBy = Guid.Parse(customer.UpdatedBy);
                return await _repository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<bool> CheckCustomerExistsAsync(string customerName)
        {
            return await _repository.CheckCustomerExistsAsync(customerName);
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

        public async Task<IEnumerable<CustomerViewModel>> GetAllCustomersAsync(string customerName, string phoneNumber, string companyId, int pageSize, int pageNumber)
        {

            var customersFromRepository = await _repository.GetAllAsync(customerName, phoneNumber, companyId, pageSize, pageNumber);

            var result = _mapper.Map<List<CustomerViewModel>>(customersFromRepository);

            return result;
        }

        public async Task<CustomerViewModel> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                var result = _mapper.Map<CustomerViewModel>(entity);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> UpdateCustomerAsync(Guid Id, CustomerRequest customer)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(Id);
                if (entity == null)
                {
                    return 0;
                }

                entity.Name = customer.Name;
                entity.RoomNumber = customer.RoomNumber;
                entity.City = customer.City;
                entity.PhoneNumber = customer.PhoneNumber;
                entity.BuildingName = customer.BuildingName;
                entity.City = customer.City;
                entity.Note = customer.Note;
                entity.PostCode1 = customer.PostCode1;
                entity.PostCode2 = customer.PostCode2;
                entity.StateProvince = customer.StateProvince;
                entity.CompanyId = customer.CompanyId;
                entity.UpdatedBy = Guid.Parse(customer.UpdatedBy);
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
