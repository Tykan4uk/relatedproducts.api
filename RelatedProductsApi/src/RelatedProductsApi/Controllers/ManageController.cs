using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RelatedProductsApi.Configurations;
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
        private readonly Config _config;

        public ManageController(
            ILogger<ManageController> logger,
            IRelatedProductService relatedProductService,
            IOptions<Config> config)
        {
            _logger = logger;
            _relatedProductService = relatedProductService;
            _config = config.Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetByPage(GetByPageRequest getByPageRequest)
        {
            var result = await _relatedProductService.GetByPageAsync(getByPageRequest.Page);
            return result != null ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(GetByIdRequest getByIdRequest)
        {
            var result = await _relatedProductService.GetByIdAsync(getByIdRequest.Id);
            return result != null ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddRequest addRequest)
        {
            var result = await _relatedProductService.AddAsync(addRequest.RelatedProduct);
            return result != null ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateRequest updateRequest)
        {
            var result = await _relatedProductService.UpdateAsync(updateRequest.RelatedProduct);
            return result != null ? Ok(result) : BadRequest(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteRequest deleteRequest)
        {
            var result = await _relatedProductService.DeleteAsync(deleteRequest.Id);
            return result != null ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPageCounter()
        {
            var result = await _relatedProductService.GetPageCounterAsync();
            return result != null ? Ok(result) : BadRequest(result);
        }
    }
}