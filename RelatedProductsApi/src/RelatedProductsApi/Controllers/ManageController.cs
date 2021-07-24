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
    [Authorize(Policy = "ApiScope")]
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

            if (result == null)
            {
                _logger.LogInformation("(ManageController/GetByPage)Null result. Bad request.");
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] GetByIdRequest request)
        {
            var result = await _relatedProductService.GetByIdAsync(request.Id);

            if (result == null)
            {
                _logger.LogInformation("(ManageController/GetById)Null result. Bad request.");
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddRequest request)
        {
            var result = await _relatedProductService.AddAsync(request.Name, request.Description, request.Price);

            if (result == null)
            {
                _logger.LogInformation("(ManageController/Add)Null result. Bad request.");
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteRequest request)
        {
            var result = await _relatedProductService.DeleteAsync(request.Id);

            if (result == null)
            {
                _logger.LogInformation("(ManageController/Delete)Null result. Bad request.");
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> PutName([FromBody] UpdateNameRequest request)
        {
            var result = await _relatedProductService.UpdateNameAsync(request.Id, request.Name);

            if (result == null)
            {
                _logger.LogInformation("(ManageController/PutName)Null result. Bad request.");
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> PutDescription([FromBody] UpdateDescriptionRequest request)
        {
            var result = await _relatedProductService.UpdateDescriptionAsync(request.Id, request.Description);

            if (result == null)
            {
                _logger.LogInformation("(ManageController/PutDescription)Null result. Bad request.");
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> PutPrice([FromBody] UpdatePriceRequest request)
        {
            var result = await _relatedProductService.UpdatePriceAsync(request.Id, request.Price);

            if (result == null)
            {
                _logger.LogInformation("(ManageController/PutPrice)Null result. Bad request.");
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}