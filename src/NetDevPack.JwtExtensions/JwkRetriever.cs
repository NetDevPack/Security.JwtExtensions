using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NetDevPack.JwtExtensions
{
    public class JwkRetriever
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public JwkRetriever(JwkOptions jwkOptions)
        {
            Options = jwkOptions;
        }

        public JwkOptions Options { get; }
        public JwkList LastResponse { get; private set; }
        public IEnumerable<SecurityKey> IssuerSigningKeyResolver(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters)
        {
            if (LastResponse == null || LastResponse.When.Add(Options.KeepFor) < DateTime.Now)
            {
                var jwkTask = GetJwks();
                jwkTask.Wait();
                LastResponse = new JwkList(jwkTask.Result);
            }

            return LastResponse.Jwks.Keys;
        }

        private async Task<JsonWebKeySet> GetJwks()
        {
            var response = await HttpClient.GetAsync(Options.JwksUri);
            var responseString = await response.Content.ReadAsStringAsync();
            return new JsonWebKeySet(responseString);

        }
    }
}