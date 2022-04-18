using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using NetDevPack.Security.Jwt.Core.Interfaces;

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

        public async Task Invoke(HttpContext httpContext, IJwtService keyService)
        {
            if (!string.IsNullOrEmpty(_jws))
            {
                await httpContext.Response.WriteAsync(_jws);
                return;
            }

            var faker = new Faker();
            var tokenHandler = new JwtSecurityTokenHandler();
            var creds = await keyService.GetCurrentSigningCredentials();
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
                SigningCredentials = creds
            };
            var jwt = tokenHandler.CreateToken(tokenDescriptor);
            _jws = tokenHandler.WriteToken(jwt);
            await httpContext.Response.WriteAsync(_jws);
        }
    }
}