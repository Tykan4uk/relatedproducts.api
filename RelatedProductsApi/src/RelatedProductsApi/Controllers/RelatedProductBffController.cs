using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RelatedProductsApi.Models.Requests;
using RelatedProductsApi.Services.Abstractions;

namespace RelatedProductsApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    [Authorize(Policy = "ApiScopeBff")]
    public class RelatedProductBffController : ControllerBase
    {
        private readonly ILogger<RelatedProductBffController> _logger;
        private readonly IRelatedProductService _relatedProductService;
        private readonly IRateLimitService _rateLimitService;

        public RelatedProductBffController(
            ILogger<RelatedProductBffController> logger,
            IRelatedProductService relatedProductService,
            IRateLimitService rateLimitService)
        {
            _logger = logger;
            _relatedProductService = relatedProductService;
            _rateLimitService = rateLimitService;
        }

        [HttpPost]
        public async Task<IActionResult> GetByPage([FromBody] GetByPageRequest request)
        {
            var result = await _relatedProductService.GetByPageAsync(request.Page, request.PageSize, request.SortedType);

            if (result == null)
            {
                _logger.LogInformation("(RelatedProductBffController/GetByPage)Null result. Bad request.");
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetById(GetByIdRequest request)
        {
            var ip = HttpContext.Connection.RemoteIpAddress;
            var url = HttpContext.Request.Path.ToUriComponent();

            var checkRateLimit = await _rateLimitService.CheckRateLimit($"{ip}{url}");
            if (checkRateLimit.CheckRateLimit)
            {
                var result = await _relatedProductService.GetByIdAsync(request.Id);

                if (result == null)
                {
                    _logger.LogInformation("(RelatedProductBffController/GetById)Null result. Bad request.");
                    return BadRequest(result);
                }

                return Ok(result);
            }

            return StatusCode(429);
        }
    }
}