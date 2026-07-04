using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Sqids;
using URLShortener.Domain.Entities;
using URLShortener.Domain.Interfaces;
using URLShortener.Infrastructure.Data;

namespace URLShortener.Application.Services
{
    public class UrlShorteningService : IUrlShorteningService
    {
        private readonly IHelperMethods _helper;
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public UrlShorteningService(IHelperMethods helper, ApplicationDbContext context, IMemoryCache cache)
        {
            _helper = helper;
            _context = context;
            _cache = cache;
        }
        public async Task<string?> GenerateCode(string url, DateTime? lifeTime, string? customAlias)
        {
            // create url entity with lifetime and alias
            var newUrl = new ShortenedURL(url, lifeTime, customAlias);

            _context.shortenedURLs.Add(newUrl);
            await _context.SaveChangesAsync();

            var code = _helper.Encode(newUrl.Id); // returns a code like "86Rf07", no collisions

            var aliasOrCode = string.IsNullOrEmpty(customAlias) ? code : customAlias;

            // Save to cache on creation to save immediate future DB hits
            var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1));

            _cache.Set(aliasOrCode, url, cacheOptions);

            return aliasOrCode;
        }

        public async Task<string?> RetrieveLongUrl(string code)
        {
            // 1. Check cache first
            if (_cache.TryGetValue(code, out string? cachedUrl))
            {
                return cachedUrl;
            }

            long? realDatabaseId = _helper.Decode(code); // returns the real database id
            ShortenedURL? mapping = null;
            if (realDatabaseId != null && realDatabaseId != -1)
            {
                mapping = await _context.shortenedURLs.FindAsync(realDatabaseId);
            }

            // Fallback for custom alias logic
            if (mapping == null)
            {
                var customLongUrl = await CustomAlias(code);
                if (customLongUrl != null)
                {
                    // Cache the custom alias result
                    _cache.Set(code, customLongUrl, TimeSpan.FromHours(1));
                    return customLongUrl;
                }
                return null;
            }

            if (mapping.IsExpired())
            {
                return "https://stackoverflow.com/questions/4677245341"; // Domain specific expiration handling
            }

            // 3. Save DB result to cache for subsequent requests
            _cache.Set(code, mapping.LongUrl, TimeSpan.FromHours(1));

            return mapping.LongUrl;
        }

        public async Task<string?> CustomAlias(string alias)
        {
            var longUrl = await _context.shortenedURLs
            .Where(u => u.CustomAlias == alias)
            .Select(u => u.LongUrl)
            .FirstOrDefaultAsync();

            return longUrl;
        }
    }
}
