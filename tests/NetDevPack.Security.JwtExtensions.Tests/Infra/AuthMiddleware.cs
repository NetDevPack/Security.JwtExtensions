using Bogus;
using Jwks.Manager;
using Jwks.Manager.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NetDevPack.Security.JwtExtensions.Tests.Infra
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private static string _jws;
        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IJsonWebKeySetService keyService, IOptions<JwksOptions> options)
        {
            if (!string.IsNullOrEmpty(_jws))
            {
                await httpContext.Response.WriteAsync(_jws);
                return;
            }

            var faker = new Faker();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = keyService.GetCurrent();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, faker.Person.UserName),
                    new Claim(JwtRegisteredClaimNames.Amr, "Fake"),
                    new Claim(JwtRegisteredClaimNames.Email, faker.Person.Email),
                    new Claim(JwtRegisteredClaimNames.GivenName, faker.Person.FullName),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Audience = "jwt-test",
                Issuer = Server.ServerUrl,
                SigningCredentials = key
            };
            var jwt = tokenHandler.CreateToken(tokenDescriptor);
            _jws = tokenHandler.WriteToken(jwt);
            await httpContext.Response.WriteAsync(_jws);
        }
    }
}