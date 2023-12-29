using CaseMngmt.Models.Companies;
using CaseMngmt.Service.Companies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CaseMngmt.Server.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly ICompanyService _service;
        public CompanyController(ILogger<CompanyController> logger, ICompanyService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet, Route("getAll")]
        public async Task<IActionResult> GetAll(string? name = null, string? phoneNumber = null, int? pageSize = 25, int? pageNumber = 1)
        {
            try
            {
                var result = await _service.GetAllAsync(name, phoneNumber, pageSize ?? 25, pageNumber ?? 1);
                return result != null && result.Any() ? Ok(result) : NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(CompanyController), true, e);
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
                CompanyViewModel? result = await _service.GetByIdAsync(id);

                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(CompanyController), true, e);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CompanyRequest Company)
        {
            if (!ModelState.IsValid || Company == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Company.CreatedBy = Guid.Parse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "00000000-0000-0000-0000-000000000000");
                var result = await _service.AddAsync(Company);

                return result > 0 ? Ok(result) : BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(CompanyController), true, e);
                return BadRequest();
            }
        }

        [HttpPut, Route("{Id}")]
        public async Task<IActionResult> Update(Guid Id, CompanyRequest model)
        {
            if (!ModelState.IsValid || Id == Guid.Empty)
            {
                return BadRequest();
            }

            try
            {
                model.UpdatedBy = Guid.Parse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "00000000-0000-0000-0000-000000000000");
                var result = await _service.UpdateAsync(Id, model);
                return result > 0 ? Ok(result) : BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(CompanyController), true, e);
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
                _logger.LogError(e.Message, nameof(CompanyController), true, e);
                return BadRequest();
            }
        }
    }
}
