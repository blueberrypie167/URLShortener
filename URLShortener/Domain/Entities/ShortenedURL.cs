using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace URLShortener.Domain.Entities
{
    [Index(nameof(CustomAlias), IsUnique = true)]
    public class ShortenedURL
    {
        public ShortenedURL() { }
        public ShortenedURL(string longUrl, DateTime? expiryDate, string? customAlias)
        {
            LongUrl = longUrl;
            CustomAlias = string.IsNullOrWhiteSpace(customAlias) ? null : customAlias;
            CreatedOn = DateTime.UtcNow;

            // Assign the provided date, or default to 30 days from now
            ExpiryDate = expiryDate ?? DateTime.UtcNow.AddDays(30);
        }

        [Key]
        public long Id { get; set; }

        [Required]
        public string LongUrl { get; set; }

        public string? CustomAlias { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ExpiryDate { get; set; } 

        public bool IsExpired() { return (ExpiryDate < DateTime.UtcNow); }
    }
}

