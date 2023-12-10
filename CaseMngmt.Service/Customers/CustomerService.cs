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

        public async Task<int> AddCustomerAsync(CustomerViewModel customer)
        {
            try
            {
                var entity = _mapper.Map<Customer>(customer);
                return await _repository.AddCustomerAsync(entity);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<bool> CheckCustomerExistsAsync(string customerName)
        {
            return await _repository.CheckCustomerExistsAsync(customerName);
        }

        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<CustomerViewModel>> GetAllCustomersAsync(string customerName, string phoneNumber, int pageSize, int pageNumber)
        {

            var customersFromRepository = await _repository.GetAllCustomersAsync(customerName, phoneNumber, pageSize, pageNumber);

            var result = _mapper.Map<List<CustomerViewModel>>(customersFromRepository);

            return result;
        }

        public async Task<CustomerViewModel> GetByIdAsync(int id)
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

        public async Task<int> UpdateCustomerAsync(CustomerViewModel customer)
        {
            try
            {
                if (customer != null)
                {
                    var entity = _mapper.Map<Customer>(customer);
                    await _repository.UpdateCustomerAsync(entity);
                    return 1;
                }

                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
