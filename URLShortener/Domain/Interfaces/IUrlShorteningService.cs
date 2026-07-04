using URLShortener.Domain.Entities;

namespace URLShortener.Domain.Interfaces
{
    public interface IUrlShorteningService
    {
        public Task<string?> GenerateCode(string url, DateTime? lifeTime, string? customAlias);

        public Task<string?> RetrieveLongUrl(string code);
    }
}
