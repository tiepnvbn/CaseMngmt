using CaseMngmt.Service.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaseMngmt.Server.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class TypeController : ControllerBase
    {
        private readonly ILogger<TypeController> _logger;
        private readonly ITypeService _typeService;


        public TypeController(ILogger<TypeController> logger, ITypeService typeService)
        {
            _logger = logger;
            _typeService = typeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var companyId = User?.FindFirst("CompanyId")?.Value;
                if (string.IsNullOrEmpty(companyId))
                {
                    return BadRequest();
                }

                var result = await _typeService.GetAllAsync();
                return result != null && result.Any() ? Ok(result) : NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, nameof(TypeController), true, e);
                return BadRequest();
            }
        }
    }
}
