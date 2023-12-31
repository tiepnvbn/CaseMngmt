﻿using CaseMngmt.Models.Templates;
using CaseMngmt.Service.Companies;
using CaseMngmt.Service.CompanyTemplates;
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
    public class TemplateController : ControllerBase
    {
        private readonly ILogger<TemplateController> _logger;
        private readonly IKeywordService _keywordService;
        private readonly ITemplateService _templateService;
        private readonly ICompanyService _companyService;
        private readonly ICompanyTemplateService _companyTemplateService;

        public TemplateController(ILogger<TemplateController> logger, IKeywordService keywordService, ITemplateService templateService, ICompanyService companyService, ICompanyTemplateService companyTemplateService)
        {
            _logger = logger;
            _keywordService = keywordService;
            _templateService = templateService;
            _companyService = companyService;
            _companyTemplateService = companyTemplateService;
        }

        [ClaimRequirement(ClaimTypes.Role, "SuperAdmin")]
        [HttpGet, Route("getAll")]
        public async Task<IActionResult> GetAll(int? pageSize = 25, int? pageNumber = 1)
        {
            try
            {
                var companyId = User?.FindFirst("CompanyId")?.Value;
                if (string.IsNullOrEmpty(companyId))
                {
                    return BadRequest();
                }
                var companyTemplate = await _companyTemplateService.GetTemplateByCompanyIdAsync(Guid.Parse(companyId));
                var templateId = companyTemplate.FirstOrDefault()?.TemplateId;
                if (templateId == null || templateId == Guid.Empty)
                {
                    return BadRequest();
                }

                var result = await _templateService.GetAllAsync(templateId, pageSize ?? 25, pageNumber ?? 1);
                return result != null && result.Any() ? Ok(result) : NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(TemplateController), true, e);
                return BadRequest();
            }
        }

        [HttpGet("template")]
        public async Task<IActionResult> GetTemplate()
        {
            try
            {
                var companyId = User?.FindFirst("CompanyId")?.Value;
                if (string.IsNullOrEmpty(companyId))
                {
                    return BadRequest();
                }
                var companyTemplate = await _companyTemplateService.GetTemplateByCompanyIdAsync(Guid.Parse(companyId));
                var templateId = companyTemplate.FirstOrDefault()?.TemplateId;
                if (templateId == null || templateId == Guid.Empty)
                {
                    return BadRequest();
                }

                TemplateViewModel? result = await _templateService.GetByIdAsync(templateId.Value);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(TemplateController), true, e);
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid templateId)
        {
            if (templateId == Guid.Empty)
            {
                return BadRequest(ModelState);
            }

            try
            {
                TemplateViewModel? result = await _templateService.GetByIdAsync(templateId);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(TemplateController), true, e);
                return BadRequest();
            }
        }

        [ClaimRequirement(ClaimTypes.Role, "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create(TemplateRequest request)
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
                // TODO : add user infomation
                var loggedUserId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

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
                _logger.LogError(e.Message, nameof(TemplateController), true, e);
                return BadRequest();
            }
        }
        
        [ClaimRequirement(ClaimTypes.Role, "SuperAdmin")]
        [HttpPut, Route("{Id}")]
        public async Task<IActionResult> Update(TemplateViewRequest request)
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
                // TODO : add user infomation
                var loggedUserId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

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
                _logger.LogError(e.Message, nameof(TemplateController), true, e);
                return BadRequest();
            }
        }

        [ClaimRequirement(ClaimTypes.Role, "SuperAdmin")]
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
                _logger.LogError(e.Message, nameof(TemplateController), true, e);
                return BadRequest();
            }
        }
    }
}
