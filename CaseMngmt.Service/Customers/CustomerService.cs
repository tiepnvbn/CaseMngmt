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
                entity.CreatedDate = DateTime.UtcNow;
                entity.UpdatedDate = DateTime.UtcNow;
                return await _repository.AddCustomerAsync(entity);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<bool> CheckCustomerExistsAsync(Guid customerId)
        {
            return await _repository.CheckCustomerExistsAsync(customerId);
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

        public async Task<IEnumerable<CustomerViewModel>> GetAllCustomersAsync(string customerName, string phoneNumber, int pageSize, int pageNumber)
        {

            var customersFromRepository = await _repository.GetAllCustomersAsync(customerName, phoneNumber, pageSize, pageNumber);

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
            catch (Exception)
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
                entity.UpdatedDate = DateTime.UtcNow;
                await _repository.UpdateCustomerAsync(entity);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
