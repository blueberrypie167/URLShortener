using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using URLShortener.Application.Models.Dtos;
using URLShortener.Application.Services;
using URLShortener.Domain.Entities;
using URLShortener.Domain.Interfaces;

namespace URLShortener.API.Controllers
{
    [ApiController]
    [Route("")]
    public class ShortURLController : Controller
    {
        private readonly IHelperMethods _helper;
        private readonly IUrlShorteningService _urlShorteningService;
        private readonly IMemoryCache _cache;

        public ShortURLController(
            IHelperMethods helper, IUrlShorteningService urlShorteningService, 
            ILogger<ShortURLController> logger, IMemoryCache memoryCache)
        {
            _helper = helper;
            _urlShorteningService = urlShorteningService;
            _cache = memoryCache;
        }

        [HttpPost("Shorten/")]
        public async Task<IActionResult> ShortenUrl(ShortURLRequest request)
        {
            if (!_helper.IsValidUrl(request.Url))
            {
                return BadRequest("Specified URL is invalid.");
            }

            var Code = await _urlShorteningService.GenerateCode(request.Url, request.LifeTime, request.CustomAlias);

            var shortUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{Code}";

            return Ok(shortUrl);
        }

        [HttpGet("/{code}")]
        public async Task<IActionResult> GetLongUrl(string code)
        {
            var longUrl = await _urlShorteningService.RetrieveLongUrl(code);

            return Redirect($"{longUrl}");
        }
    }
}
