using Sqids;
using URLShortener.Domain.Interfaces;

namespace URLShortener.Application.Services
{
    public class HelperMethods : IHelperMethods
    {
        private const long SevenCharOffset = 56800235584L;

        private readonly SqidsEncoder<long> _encoder;

        public HelperMethods(SqidsEncoder<long> encoder)
        {
            _encoder = encoder;
        }
        public bool IsValidUrl(string urlString)
        {
            bool isUri = Uri.TryCreate(urlString, UriKind.Absolute, out Uri? validatedUri);

            return isUri && (validatedUri?.Scheme == Uri.UriSchemeHttp || validatedUri?.Scheme == Uri.UriSchemeHttps);
        }

        public string Encode(long databaseId)
        {
            return _encoder.Encode(SevenCharOffset + databaseId);
        }
        public long? Decode(string code)
        {
            var numbers = _encoder.Decode(code);
            if (numbers.Count > 0)
            {
                return numbers[0] - SevenCharOffset;
            }
            return -1;
        }
    }
}
