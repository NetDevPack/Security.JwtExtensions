using System;

namespace NetDevPack.Security.JwtExtensions
{
    public class JwkOptions
    {
        public JwkOptions(string jwksUri)
        {
            JwksUri = new Uri(jwksUri);
            Issuer = $"{JwksUri.Scheme}://{JwksUri.Authority}";
            KeepFor = TimeSpan.FromMinutes(15);
        }

        public JwkOptions(string jwksUri, TimeSpan cacheTime)
        {
            JwksUri = new Uri(jwksUri);
            Issuer = $"{JwksUri.Scheme}://{JwksUri.Authority}";
            KeepFor = cacheTime;
        }

        public JwkOptions(string jwksUri, string issuer, TimeSpan cacheTime)
        {
            JwksUri = new Uri(jwksUri);
            Issuer = issuer;
            KeepFor = cacheTime;
        }
        public string Issuer { get; private set; }
        public Uri JwksUri { get; private set; }
        public TimeSpan KeepFor { get; private set; }

    }
}