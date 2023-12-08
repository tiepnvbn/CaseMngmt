using CaseMngmt.Server.Models.Customers;
using CaseMngmt.Server.Service;
using Microsoft.AspNetCore.Mvc;

namespace CaseMngmt.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
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
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllCustomersAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(CustomerController), true, e);
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Customer result = await _service.GetByIdAsync(id.Value);

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

        [HttpPost, Route("create")]
        public async Task<IActionResult> Create(Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // TODO
                    customer.CreatedBy = 0;
                    customer.CreatedDate = DateTime.UtcNow;

                    var result = await _service.AddCustomerAsync(customer);
                    return Ok(result);
                }

                return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(CustomerController), true, e);
                return BadRequest();
            }
        }

        [HttpPut, Route("update/{newsId}")]
        public async Task<IActionResult> Update(Guid newsId, Customer model)
        {
            try
            {
                // TODO
                model.UpdatedBy = 0;
                model.UpdatedDate = DateTime.UtcNow;

                var result = await _service.UpdateCustomerAsync(model);
                return result > 0 ? Ok(result) : BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(CustomerController), true, e);
                return BadRequest();
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
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
