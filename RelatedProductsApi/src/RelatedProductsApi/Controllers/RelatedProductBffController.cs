﻿using System.Threading.Tasks;
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
    public class RelatedProductBffController : ControllerBase
    {
        private readonly ILogger<RelatedProductBffController> _logger;
        private readonly IRelatedProductService _relatedProductService;
        private readonly Config _config;

        public RelatedProductBffController(
            ILogger<RelatedProductBffController> logger,
            IOptions<Config> config,
            IRelatedProductService relatedProductService)
        {
            _logger = logger;
            _config = config.Value;
            _relatedProductService = relatedProductService;
        }

        [HttpPost]
        public async Task<IActionResult> GetByPage(GetByPageRequest getByPageRequest)
        {
            var result = await _relatedProductService.GetByPageAsync(getByPageRequest);
            return result != null ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetById(GetByIdRequest getByIdRequest)
        {
            var result = await _relatedProductService.GetByIdAsync(getByIdRequest);
            return result != null ? Ok(result) : BadRequest(result);
        }
    }
}