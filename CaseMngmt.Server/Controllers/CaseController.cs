using CaseMngmt.Models;
using CaseMngmt.Models.CaseKeywords;
using CaseMngmt.Models.FileUploads;
using CaseMngmt.Service.CaseKeywords;
using CaseMngmt.Service.CompanyTemplates;
using CaseMngmt.Service.FileUploads;
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
        private readonly IFileUploadService _fileUploadService;
        private readonly ICompanyTemplateService _companyTemplateService;
        private readonly IConfiguration _configuration;

        public CaseController(ILogger<CaseController> logger, ICaseKeywordService caseKeywordService,
            ITemplateService templateService, IFileUploadService fileUploadService, ICompanyTemplateService companyTemplateService, IConfiguration configuration)
        {
            _logger = logger;
            _caseKeywordService = caseKeywordService;
            _templateService = templateService;
            _fileUploadService = fileUploadService;
            _companyTemplateService = companyTemplateService;
            _configuration = configuration;
        }

        [HttpPost, Route("getAll")]
        public async Task<IActionResult> GetAll(CaseKeywordSearch request)
        {
            if (!ModelState.IsValid || request == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Get Template to check role of user
                var currentUserRole = User?.FindAll(ClaimTypes.Role)?.Select(x => x.Value)?.ToList();
                var currentCompanyId = User?.FindFirst("CompanyId")?.Value;
                if (currentUserRole == null || currentUserRole.Count < 1 || string.IsNullOrEmpty(currentCompanyId))
                {
                    return BadRequest("Wrong Claim");
                }

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

                var searchRequest = new CaseKeywordSearchRequest
                {
                    CompanyId = Guid.Parse(currentCompanyId),
                    TemplateId = templateId,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    KeywordValues = request.KeywordValues
                };

                var result = await _caseKeywordService.GetAllAsync(searchRequest);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(CaseController), true, e);
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
                _logger.LogError(e.Message, nameof(CaseController), true, e);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Close")]
        public async Task<IActionResult> CloseCase(Guid caseId)
        {
            if (caseId == Guid.Empty)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _caseKeywordService.CloseCaseByAsync(caseId);

                return result > 0 ? Ok(result) : BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(CaseController), true, e);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("file/getall")]
        public async Task<IActionResult> GetAllFileByCaseId(Guid caseId)
        {
            if (caseId == Guid.Empty)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var fileKeywordsResult = await _caseKeywordService.GetFileKeywordsByCaseIdAsync(caseId);
                AWSSetting? awsSetting = null;
                if (!string.IsNullOrEmpty(_configuration["AWS:S3Bucket"]))
                {
                    awsSetting = new AWSSetting()
                    {
                        S3Bucket = _configuration["AWS:S3Bucket"],
                        ACCESS_KEY = _configuration["AWS:ACCESS_KEY"],
                        SECRET_KEY = _configuration["AWS:SECRET_KEY"],
                        UploadFolder = _configuration["AWS:UploadFolder"]
                    };
                }
                var fileSetting = new FileUploadSetting()
                {
                    AcceptTypes = _configuration["FileUploadSettings:acceptTypes"],
                    InvalidFileExtensions = _configuration["FileUploadSettings:invalidFileExtensions"],
                    UploadFolder = _configuration["FileUploadSettings:uploadFolder"],
                    ValidFileTypes = _configuration["FileUploadSettings:validFileTypes"],
                };

                var availableFileResult = await _fileUploadService.GetAllFileByCaseIdAsync(caseId, fileSetting, awsSetting);

                var result = fileKeywordsResult.ToList();
                if (availableFileResult != null && fileKeywordsResult.Count() != availableFileResult.Count())
                {
                    result = fileKeywordsResult.Where(x => availableFileResult.Contains(x.FilePath)).ToList();
                }

                return result.Any()
                    ? Ok(result)
                    : NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(CaseController), true, e);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CaseKeywordAddRequest request)
        {
            if (!ModelState.IsValid || request == null)
            {
                return BadRequest(ModelState);
            }

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

                var userTemplate = await _templateService.GetByIdAsync(templateId.Value);
                if (userTemplate == null)
                {
                    return BadRequest();
                }

                var isInValidModel = request.KeywordValues.Any(x => !x.IsValidModel());
                if (isInValidModel)
                {
                    return BadRequest("KeywordValues is wrong format");
                }

                var userKeywordSetting = userTemplate.Keywords.Select(x => x.KeywordId).ToList();
                var requestKeywords = request.KeywordValues.Select(x => x.KeywordId).ToList();

                var isValidKeywords = userKeywordSetting.All(requestKeywords.Contains) && userKeywordSetting.Count == requestKeywords.Count;
                if (!isValidKeywords)
                {
                    return BadRequest("KeywordValues is wrong");
                }

                var result = await _caseKeywordService.AddAsync(request);

                return result != null ? Ok(result) : BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(CaseController), true, e);
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(CaseKeywordRequest request)
        {
            if (!ModelState.IsValid || request == null || request.CaseId == Guid.Empty)
            {
                return BadRequest();
            }

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

                var userTemplate = await _templateService.GetByIdAsync(templateId.Value);
                if (userTemplate == null)
                {
                    return BadRequest();
                }

                var isInValidModel = request.KeywordValues.Any(x => !x.IsValidModel());
                if (isInValidModel)
                {
                    return BadRequest("KeywordValues is wrong format");
                }

                var userKeywordSetting = userTemplate.Keywords.Select(x => x.KeywordId).ToList();
                var requestKeywords = request.KeywordValues.Select(x => x.KeywordId).ToList();

                var allOfUserKeywordsIsInRequest = userKeywordSetting.Intersect(requestKeywords).Count() == userKeywordSetting.Count();
                if (!allOfUserKeywordsIsInRequest)
                {
                    return BadRequest("KeywordValues is wrong");
                }

                var result = await _caseKeywordService.UpdateAsync(request);
                return result > 0 ? Ok(result) : BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(CaseController), true, e);
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
                _logger.LogError(e.Message, nameof(CaseController), true, e);
                return BadRequest();
            }
        }
    }
}
