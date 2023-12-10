using CaseMngmt.Models.Customers;
using CaseMngmt.Service.Customers;
using Microsoft.AspNetCore.Mvc;

namespace CaseMngmt.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _service;
        public CustomerController(ILogger<CustomerController> logger, ICustomerService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet, Route("getAll")]
        public async Task<IActionResult> GetAll(string? customerName = null, string? phoneNumber = null, int? pageSize = 25, int? pageNumber = 1)
        {
            try
            {
                var result = await _service.GetAllCustomersAsync(customerName, phoneNumber, pageSize.Value, pageNumber.Value);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(CustomerController), true, e);
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(ModelState);
            }

            try
            {
                CustomerViewModel result = await _service.GetByIdAsync(id);

                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(CustomerController), true, e);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomerRequest customer)
        {
            if (!ModelState.IsValid || customer == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var isExist = await _service.CheckCustomerExistsAsync(customer.Name);
                if (isExist)
                {
                    return BadRequest("Customer already exists");
                }
                // TODO
                // customer.CreatedBy = Guid.Empty;

                var result = await _service.AddCustomerAsync(customer);

                return result > 0 ? Ok(result) : BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(CustomerController), true, e);
                return BadRequest();
            }
        }

        [HttpPut, Route("{Id}")]
        public async Task<IActionResult> Update(Guid Id, CustomerRequest model)
        {
            if (Id == Guid.Empty)
            {
                return BadRequest();
            }

            try
            {
                var isExist = await _service.CheckCustomerExistsAsync(model.Name);
                if (isExist)
                {
                    return BadRequest("Customer already exists");
                }

                // TODO
                // model.UpdatedBy = Guid.Empty;

                var result = await _service.UpdateCustomerAsync(Id, model);
                return result > 0 ? Ok(result) : BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(CustomerController), true, e);
                return BadRequest();
            }
        }

        [HttpDelete, Route("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _service.DeleteAsync(id);
                return result > 0 ? Ok(result) : BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(CustomerController), true, e);
                return BadRequest();
            }
        }
    }
}
