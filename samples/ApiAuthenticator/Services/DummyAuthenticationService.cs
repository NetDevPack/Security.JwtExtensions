using Bogus;
using Jwks.Manager.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiAuthenticator.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IJsonWebKeySetService _jsonWebKeySetService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(IConfiguration configuration, IJsonWebKeySetService jsonWebKeySetService, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _jsonWebKeySetService = jsonWebKeySetService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async ValueTask<object> GenerateToken()
        {

            var faker = new Faker();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = _jsonWebKeySetService.GetCurrent();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, faker.Person.UserName),
                    new Claim(JwtRegisteredClaimNames.Amr, "Fake"),
                    new Claim(JwtRegisteredClaimNames.Email, faker.Person.Email),
                    new Claim(JwtRegisteredClaimNames.GivenName, faker.Person.FullName),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}",
                SigningCredentials = key
            };
            var jwt = tokenHandler.CreateToken(tokenDescriptor);
            var jws = tokenHandler.WriteToken(jwt);


            return await Task.FromResult(jws);
        }

        public async ValueTask<bool> SignInAsync(string login, string password)
        {
            var faker = new Faker();

            if (string.IsNullOrEmpty(login))
            {
                return false;
            }

            if (password.Length < 3)
            {
                return false;
            }

            return await Task.FromResult(true);
        }
    }
}
