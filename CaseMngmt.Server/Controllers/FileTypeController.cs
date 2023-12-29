using CaseMngmt.Service.FileTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaseMngmt.Server.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class FileTypeController : ControllerBase
    {
        private readonly ILogger<TemplateController> _logger;
        private readonly IFileTypeService _fileTypeService;

        public FileTypeController(ILogger<TemplateController> logger, IFileTypeService fileTypeService)
        {
            _logger = logger;
            _fileTypeService = fileTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int? pageSize = 25, int? pageNumber = 1)
        {
            try
            {
                var result = await _fileTypeService.GetAllAsync(pageSize ?? 25, pageNumber ?? 1);
                return result != null && result.Any() ? Ok(result) : NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(FileTypeController), true, e);
                return BadRequest();
            }
        }
    }
}
