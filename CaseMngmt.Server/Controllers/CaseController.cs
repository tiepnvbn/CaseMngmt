using CaseMngmt.Models.CaseKeywords;
using CaseMngmt.Service.CaseKeywords;
using CaseMngmt.Service.Templates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CaseMngmt.Server.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class CaseController : ControllerBase
    {
        private readonly ILogger<CaseController> _logger;
        private readonly ICaseKeywordService _caseKeywordService;
        private readonly ITemplateService _templateService;
        public CaseController(ILogger<CaseController> logger, ICaseKeywordService caseKeywordService, ITemplateService templateService)
        {
            _logger = logger;
            _caseKeywordService = caseKeywordService;
            _templateService = templateService;
        }

        [HttpGet, Route("getAll")]
        public async Task<IActionResult> GetAll(CaseKeywordSearchRequest searchRequest)
        {
            if (!ModelState.IsValid || searchRequest == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Get Template to check role of user
                var currentUserRole = User.FindAll(ClaimTypes.Role).Select(x => x.Value).ToList();
                if (currentUserRole == null || currentUserRole.Count < 1)
                {
                    return BadRequest("Wrong User Role");
                }

                var currentCompanyId = User.FindFirst("CompanyId").Value;
                searchRequest.CompanyId = Guid.Parse(currentCompanyId);
                var result = await _caseKeywordService.GetAllAsync(searchRequest);
                return Ok(result);
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
                CaseKeywordViewModel result = await _caseKeywordService.GetByIdAsync(caseId);

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

        // TODO : integrate with image/file
        [HttpPost]
        public async Task<IActionResult> Create(CaseKeywordRequest request)
        {
            if (!ModelState.IsValid || request == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _caseKeywordService.AddAsync(request);

                return result > 0 ? Ok(result) : BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(CustomerController), true, e);
                return BadRequest();
            }
        }

        [HttpPut, Route("{Id}")]
        public async Task<IActionResult> Update(CaseKeywordRequest request)
        {
            if (!ModelState.IsValid || request == null || request.CaseId == Guid.Empty)
            {
                return BadRequest();
            }

            try
            {
                var result = await _caseKeywordService.UpdateAsync(request);
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
                var result = await _caseKeywordService.DeleteAsync(id);
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
