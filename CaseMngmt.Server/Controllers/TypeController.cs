﻿using CaseMngmt.Service.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CaseMngmt.Server.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ClaimRequirement(ClaimTypes.Role, "SuperAdmin")]
    [ApiController]
    [Route("api/[controller]")]
    public class TypeController : ControllerBase
    {
        private readonly ILogger<TemplateController> _logger;
        private readonly ITypeService _typeService;


        public TypeController(ILogger<TemplateController> logger, ITypeService typeService)
        {
            _logger = logger;
            _typeService = typeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int? pageSize = 25, int? pageNumber = 1)
        {
            try
            {
                var companyId = User?.FindFirst("CompanyId")?.Value;
                if (string.IsNullOrEmpty(companyId))
                {
                    return BadRequest();
                }

                var result = await _typeService.GetAllAsync(pageSize ?? 25, pageNumber ?? 1);
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