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

        public FileUploadController(ILogger<FileUploadController> logger, IFileUploadService fileUploadService, ICaseKeywordService caseKeywordService, ICompanyTemplateService companyTemplateService)
        {
            _logger = logger;
            _fileUploadService = fileUploadService;
            _caseKeywordService = caseKeywordService;
            _companyTemplateService = companyTemplateService;
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

                if (fileUploadRequest.Validate())
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

                if (string.IsNullOrEmpty(fileUploadRequest.FileName))
                {
                    fileUploadRequest.FileName = fileUploadRequest.FileToUpload.FileName;
                }
                string ext = Path.GetExtension(fileUploadRequest.FileToUpload.FileName).ToLower();

                var filePath = GetFilePath($"{fileUploadRequest.FileName}{ext}", fileUploadRequest.CaseId);
                var uploadResult = await _fileUploadService.UploadFileAsync(fileUploadRequest.FileToUpload, filePath);

                if (uploadResult > 0)
                {
                    var result = await _caseKeywordService.AddFileToKeywordAsync(fileUploadRequest, templateId.Value);
                    return result > 0 ? Ok() : BadRequest();
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

                var filePath = GetFilePath(filename, caseId);
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(filePath, out var contenttype))
                {
                    contenttype = "application/octet-stream";
                }

                var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
                return File(bytes, contenttype, Path.GetFileName(filePath));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(FileUploadController), true, e);
                return BadRequest();
            }
        }

        private string GetFilePath(string filename, Guid caseId)
        {
            var fileSetting = new FileUploadSettings();
            var uploadFolder = fileSetting.UploadFolder;

            var filepath = Path.Combine(Directory.GetCurrentDirectory(), $"{uploadFolder}\\{caseId}");
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            string ext = Path.GetExtension(filename).ToLower();
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filename);
            var exactpath = Path.Combine(filepath, fileNameWithoutExt + ext);

            return exactpath;
        }
    }
}
