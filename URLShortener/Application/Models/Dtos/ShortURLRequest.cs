using System.ComponentModel.DataAnnotations;

namespace URLShortener.Application.Models.Dtos
{
    public class ShortURLRequest
    {
        [Required]
        public string Url { get; set; } = string.Empty;

        // Accept string representation from the dropdown
        public string? LifetimeString { get; set; }

        [StringLength(20, MinimumLength = 3)]
        [RegularExpression(@"^(?=.*[^a-zA-Z0-9]).+$", ErrorMessage = "The input must contain at least one special character.")]
        public string? CustomAlias { get; set; }
    }
}
