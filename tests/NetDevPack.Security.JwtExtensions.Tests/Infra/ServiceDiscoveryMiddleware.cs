using Jwks.Manager;
using Jwks.Manager.Interfaces;
using Jwks.Manager.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace NetDevPack.Security.JwtExtensions.Tests.Infra
{
    public class ServiceDiscoveryMiddleware
    {
        private readonly RequestDelegate _next;
        public static int CallTimes;
        public ServiceDiscoveryMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IJsonWebKeySetService keyService, IOptions<JwksOptions> options)
        {
            var keys = new
            {
                keys = keyService.GetLastKeysCredentials(options.Value.AlgorithmsToKeep)?.Select(PublicJsonWebKey.FromJwk)
            };

            Interlocked.Increment(ref CallTimes);
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(keys, new JsonSerializerOptions() { IgnoreNullValues = true }));
        }
    }
}