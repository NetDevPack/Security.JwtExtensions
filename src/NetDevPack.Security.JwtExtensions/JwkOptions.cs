using System;

namespace NetDevPack.Security.JwtExtensions
{
    public class JwkOptions
    {
        public JwkOptions() { }
        public JwkOptions(string jwksUri, string issuer = null, TimeSpan? cacheTime = null, string audience = null)
        {
            JwksUri = jwksUri;
            var jwks = new Uri(jwksUri);
            Issuer = issuer ?? $"{jwks.Scheme}://{jwks.Authority}";
            KeepFor = cacheTime ?? TimeSpan.FromMinutes(15);
            Audience = audience;
        }
        public string Issuer { get; set; }
        public string JwksUri { get; set; }
        public TimeSpan KeepFor { get; set; } = TimeSpan.FromMinutes(15);
        public string Audience { get; set; }
    }
}