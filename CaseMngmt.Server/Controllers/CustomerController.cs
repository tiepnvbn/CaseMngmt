using CaseMngmt.Models.Customers;
using CaseMngmt.Service.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CaseMngmt.Server.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    [ClaimRequirement(ClaimTypes.Role, "SuperAdmin")]
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
                var currentCompanyId = User.FindFirst("CompanyId").Value;
                var result = await _service.GetAllCustomersAsync(customerName, phoneNumber, currentCompanyId, pageSize.Value, pageNumber.Value);
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
                customer.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                customer.CompanyId = Guid.Parse(User.FindFirst("CompanyId").Value);
                var result = await _service.AddCustomerAsync(customer);

                return result > 0 ? Ok(result) : BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(CustomerController), true, e);
                return BadRequest();
            }
        }

        [HttpPut, Route("{id}")]
        public async Task<IActionResult> Update(Guid id, CustomerRequest model)
        {
            if (!ModelState.IsValid || id == Guid.Empty)
            {
                return BadRequest();
            }

            try
            {
                model.CompanyId = Guid.Parse(User.FindFirst("CompanyId").Value);
                model.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var result = await _service.UpdateCustomerAsync(id, model);
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
