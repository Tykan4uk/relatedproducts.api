using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RelatedProductsApi.Models.Requests;
using RelatedProductsApi.Services.Abstractions;

namespace RelatedProductsApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class ManageController : ControllerBase
    {
        private readonly ILogger<ManageController> _logger;
        private readonly IRelatedProductService _relatedProductService;

        public ManageController(
            ILogger<ManageController> logger,
            IRelatedProductService relatedProductService)
        {
            _logger = logger;
            _relatedProductService = relatedProductService;
        }

        [HttpGet]
        public async Task<IActionResult> GetByPage([FromQuery] GetByPageRequest request)
        {
            var result = await _relatedProductService.GetByPageAsync(request.Page, request.PageSize, request.SortedType);
            return result != null ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] GetByIdRequest request)
        {
            var result = await _relatedProductService.GetByIdAsync(request.Id);
            return result != null ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddRequest request)
        {
            var result = await _relatedProductService.AddAsync(request.Name, request.Description, request.Price);
            return result != null ? Ok(result) : BadRequest(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteRequest request)
        {
            var result = await _relatedProductService.DeleteAsync(request.Id);
            return result != null ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> PutName([FromBody] UpdateNameRequest request)
        {
            var result = await _relatedProductService.UpdateNameAsync(request.Id, request.Name);
            return result != null ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> PutDescription([FromBody] UpdateDescriptionRequest request)
        {
            var result = await _relatedProductService.UpdateDescriptionAsync(request.Id, request.Description);
            return result != null ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> PutPrice([FromBody] UpdatePriceRequest request)
        {
            var result = await _relatedProductService.UpdatePriceAsync(request.Id, request.Price);
            return result != null ? Ok(result) : BadRequest(result);
        }
    }
}