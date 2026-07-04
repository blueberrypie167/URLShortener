namespace URLShortener.Domain.Interfaces
{
    public interface IHelperMethods
    {
        public bool IsValidUrl(string urlString);
        public string Encode(long urlDatabaseId);
        public long? Decode(string code);
    }
}
