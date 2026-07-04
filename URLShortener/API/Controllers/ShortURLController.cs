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
        // Added [FromForm] to parse form data sent by HTMX
        public async Task<IActionResult> ShortenUrl([FromForm] ShortURLRequest request) 
        {
            if (!_helper.IsValidUrl(request.Url))
            {
                return BadRequest("<div style='color:red;'>Specified URL is invalid.</div>");
            }

            // Convert string lifecycle value to a proper expiration date
            DateTime? lifeTimeDate = request.LifetimeString switch
            {
                "1hr" => DateTime.UtcNow.AddHours(1),
                "7hr" => DateTime.UtcNow.AddHours(7),
                "24hr" => DateTime.UtcNow.AddHours(24),
                "7d" => DateTime.UtcNow.AddDays(7),
                "30d" => DateTime.UtcNow.AddDays(30),
                "Never" => null,
                _ => DateTime.UtcNow.AddDays(30) // Default fallback
            };

            var code = await _urlShorteningService.GenerateCode(request.Url, lifeTimeDate, request.CustomAlias);

            var shortUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{code}";

            // Return html anchor tag for HTMX to elegantly insert
            string htmlResponse = $"Success! Your shortened URL is: <a href='{shortUrl}' target='_blank'>{shortUrl}</a>";

            return Content(htmlResponse, "text/html");
        }

        [HttpGet("/{code}")]
        public async Task<IActionResult> GetLongUrl(string code)
        {
            var longUrl = await _urlShorteningService.RetrieveLongUrl(code);

            return Redirect($"{longUrl}");
        }
    }
}
