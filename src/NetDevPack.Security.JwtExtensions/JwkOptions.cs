using System;

namespace NetDevPack.Security.JwtExtensions
{
    public class JwkOptions
    {
        public JwkOptions(string jwksUri, string issuer = null, TimeSpan? cacheTime = null, string audience = null)
        {
            JwksUri = new Uri(jwksUri);
            Issuer = issuer ??  $"{JwksUri.Scheme}://{JwksUri.Authority}";
            KeepFor = cacheTime ?? TimeSpan.FromMinutes(15);
            Audience = audience;
        }
        public string Issuer { get; }
        public Uri JwksUri { get; }
        public TimeSpan KeepFor { get; }
        public string Audience { get; }
    }
}