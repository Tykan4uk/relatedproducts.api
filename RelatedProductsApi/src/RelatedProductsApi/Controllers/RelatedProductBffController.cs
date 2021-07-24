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

        public RelatedProductBffController(
            ILogger<RelatedProductBffController> logger,
            IRelatedProductService relatedProductService)
        {
            _logger = logger;
            _relatedProductService = relatedProductService;
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
            var result = await _relatedProductService.GetByIdAsync(request.Id);

            if (result == null)
            {
                _logger.LogInformation("(RelatedProductBffController/GetById)Null result. Bad request.");
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}