using CaseMngmt.Models.Keywords;
using CaseMngmt.Models.Templates;
using CaseMngmt.Service.Companies;
using CaseMngmt.Service.Keywords;
using CaseMngmt.Service.Templates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CaseMngmt.Server.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    [ClaimRequirement(ClaimTypes.Role, "Admin")]
    public class TemplateController : ControllerBase
    {
        private readonly ILogger<TemplateController> _logger;
        private readonly IKeywordService _keywordService;
        private readonly ITemplateService _templateService;
        private readonly ICompanyService _companyService;

        public TemplateController(ILogger<TemplateController> logger, IKeywordService keywordService, ITemplateService templateService, ICompanyService companyService)
        {
            _logger = logger;
            _keywordService = keywordService;
            _templateService = templateService;
            _companyService = companyService;
        }

        [HttpGet, Route("getAll")]
        public async Task<IActionResult> GetAll(int? pageSize = 25, int? pageNumber = 1)
        {
            try
            {
                // Get Template to check role of user
                var currentTemplateId = User.FindFirst("TemplateId")?.Value;
                if (string.IsNullOrEmpty(currentTemplateId))
                {
                    return BadRequest("Wrong Claim");
                }

                var templateId = Guid.Parse(currentTemplateId);

                var result = await _templateService.GetAllAsync(templateId, pageSize.Value, pageNumber.Value);
                return result != null ? Ok(result) : BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(CustomerController), true, e);
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid caseId)
        {
            if (caseId == Guid.Empty)
            {
                return BadRequest(ModelState);
            }

            try
            {
                KeywordViewModel? result = await _keywordService.GetByIdAsync(caseId);

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
        public async Task<IActionResult> Create(TemplateAddRequest request)
        {
            if (!ModelState.IsValid || request == null)
            {
                return BadRequest(ModelState);
            }
            if (request.KeywordRequests.Count <= 0)
            {
                return BadRequest("Keywords is required.");
            }

            try
            {
                var company = await _companyService.GetByIdAsync(request.CompanyId);
                if (company == null)
                {
                    return BadRequest();
                }

                var result = await _templateService.AddAsync(request);

                return result > 0 ? Ok(result) : BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(CustomerController), true, e);
                return BadRequest();
            }
        }
        // TODO : integrate with image/file
        [HttpPut, Route("{Id}")]
        public async Task<IActionResult> Update(TemplateRequest request)
        {
            if (!ModelState.IsValid || request == null || request.TemplateId == Guid.Empty)
            {
                return BadRequest();
            }
            if (request.KeywordRequests.Count <= 0)
            {
                return BadRequest("Keywords is required.");
            }

            try
            {
                var company = await _companyService.GetByIdAsync(request.CompanyId);
                if (company == null)
                {
                    return BadRequest();
                }

                var result = await _templateService.UpdateAsync(request);
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
                var result = await _templateService.DeleteAsync(id);
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
