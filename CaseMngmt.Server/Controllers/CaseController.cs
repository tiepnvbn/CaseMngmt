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
                var currentCompanyId = User.FindFirst("CompanyId")?.Value;
                var currentTemplateId = User.FindFirst("TemplateId")?.Value;
                if (currentUserRole == null || currentUserRole.Count < 1 || string.IsNullOrEmpty(currentCompanyId) || string.IsNullOrEmpty(currentTemplateId))
                {
                    return BadRequest("Wrong Claim");
                }

                searchRequest.CompanyId = Guid.Parse(currentCompanyId);
                searchRequest.TemplateId = Guid.Parse(currentTemplateId);

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
                CaseKeywordViewModel? result = await _caseKeywordService.GetByIdAsync(caseId);

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
        public async Task<IActionResult> Create(CaseKeywordAddRequest request)
        {
            if (!ModelState.IsValid || request == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userTemplate = await _templateService.GetByIdAsync(request.TemplateId);
                if (userTemplate == null)
                {
                    return BadRequest();
                }

                var userKeywordSetting = userTemplate.Keywords.Select(x => x.Id).ToList();
                var requestKeywords = request.KeywordValues.Select(x => x.KeywordId).ToList();

                var isSameKeyword = userKeywordSetting.All(requestKeywords.Contains);
                if (!isSameKeyword)
                {
                    return BadRequest("KeywordValues is wrong");
                }

                var result = await _caseKeywordService.AddAsync(request);

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
