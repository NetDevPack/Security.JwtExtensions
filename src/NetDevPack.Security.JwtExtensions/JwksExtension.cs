using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace NetDevPack.Security.JwtExtensions
{
    public static class JwksExtension
    {
        public static void SetJwksOptions(this JwtBearerOptions options, JwkOptions jwkOptions)
        {
            options.Authority = null;
            options.Audience = null;

            if (options.TokenValidationParameters == null)
                options.TokenValidationParameters = new TokenValidationParameters();

            if (options.TokenValidationParameters.IssuerSigningKeyResolver == null)
                options.TokenValidationParameters.IssuerSigningKeyResolver = new JwkRetriever(jwkOptions).IssuerSigningKeyResolver;

            options.TokenValidationParameters.ValidateAudience = false;
            options.TokenValidationParameters.ValidIssuer = jwkOptions.Issuer;
        }
    }
}
