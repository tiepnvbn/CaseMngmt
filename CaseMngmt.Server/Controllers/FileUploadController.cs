using CaseMngmt.Models;
using CaseMngmt.Models.CaseKeywords;
using CaseMngmt.Models.FileUploads;
using CaseMngmt.Service.CaseKeywords;
using CaseMngmt.Service.CompanyTemplates;
using CaseMngmt.Service.FileUploads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CaseMngmt.Server.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : ControllerBase
    {
        private readonly ILogger<FileUploadController> _logger;
        private readonly IFileUploadService _fileUploadService;
        private readonly ICaseKeywordService _caseKeywordService;
        private readonly ICompanyTemplateService _companyTemplateService;
        private readonly IConfiguration _configuration;

        public FileUploadController(ILogger<FileUploadController> logger, IFileUploadService fileUploadService, ICaseKeywordService caseKeywordService, ICompanyTemplateService companyTemplateService, IConfiguration configuration)
        {
            _logger = logger;
            _fileUploadService = fileUploadService;
            _caseKeywordService = caseKeywordService;
            _companyTemplateService = companyTemplateService;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> UploadFile([FromForm] CaseKeywordFileUpload fileUploadRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (fileUploadRequest.FileToUpload == null)
                {
                    return BadRequest();
                }

                var awsSetting = GetAWSSetting();
                var fileSetting = GetFileUploadSetting();

                if (!fileUploadRequest.Validate(fileSetting))
                {
                    return BadRequest("Your file is not supported");
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

                var uploadResult = await _fileUploadService.UploadFileAsync(fileUploadRequest.FileToUpload, fileUploadRequest.CaseId, fileSetting, awsSetting);
                if (uploadResult != null)
                {
                    var result = await _caseKeywordService.AddFileToKeywordAsync(fileUploadRequest.CaseId, uploadResult, templateId.Value);
                    return result != null ? Ok(new FileResponse
                    {
                        FileName = uploadResult.FileName,
                        FilePath = uploadResult.FilePath,
                        KeywordId = result.Value
                    }) : BadRequest();
                }

                return BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(FileUploadController), true, e);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Download")]
        public async Task<IActionResult> DownloadFile(string filename, Guid caseId)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(filename))
            {
                return BadRequest();
            }
            try
            {
                string ext = Path.GetExtension(filename).ToLower();
                if (string.IsNullOrEmpty(ext))
                {
                    return BadRequest("The filename need file type");
                }

                var awsSetting = GetAWSSetting();
                var fileSetting = GetFileUploadSetting();

                if (awsSetting == null)
                {
                    var filePath = await _fileUploadService.GetFilePath(filename, caseId, fileSetting, awsSetting);

                    var provider = new FileExtensionContentTypeProvider();
                    if (!provider.TryGetContentType(filePath, out var contenttype))
                    {
                        contenttype = "application/octet-stream";
                    }

                    var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
                    return File(bytes, contenttype, Path.GetFileName(filePath));
                }
                else
                {
                    // TODO
                    return null;
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(FileUploadController), true, e);
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteFile(string filename, Guid keywordId, Guid caseId)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(filename))
            {
                return BadRequest();
            }
            try
            {
                string ext = Path.GetExtension(filename).ToLower();
                if (string.IsNullOrEmpty(ext))
                {
                    return BadRequest("The filename need file type");
                }

                var awsSetting = GetAWSSetting();
                var fileSetting = GetFileUploadSetting();

                var deleteResult = await _fileUploadService.DeleteFileAsync(filename, caseId, fileSetting, awsSetting);
                if (deleteResult > 0)
                {
                    var result = await _caseKeywordService.DeleteFileKeywordAsync(caseId, keywordId);
                    return result > 0 ? Ok(result) : BadRequest();
                }

                return BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(FileUploadController), true, e);
                return BadRequest();
            }
        }

        private AWSSetting? GetAWSSetting()
        {
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
            return awsSetting;
        }

        private FileUploadSetting GetFileUploadSetting()
        {
            var fileSetting = new FileUploadSetting()
            {
                AcceptTypes = _configuration["FileUploadSettings:acceptTypes"],
                InvalidFileExtensions = _configuration["FileUploadSettings:invalidFileExtensions"],
                UploadFolder = _configuration["FileUploadSettings:uploadFolder"],
                ValidFileTypes = _configuration["FileUploadSettings:validFileTypes"],
            };
            return fileSetting;
        }
    }
}
