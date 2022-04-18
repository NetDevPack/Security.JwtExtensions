using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace NetDevPack.Security.JwtExtensions.Tests.Infra
{
    public class Server : IDisposable
    {
        public const string ServerUrl = "https://localhost:5001";
        public readonly IWebHost TestServer;
        public readonly HttpClient HttpClient = new HttpClient() { BaseAddress = new Uri(ServerUrl) };

        public Server()
        {
            TestServer =
                new WebHostBuilder()
                .UseKestrel()
                .ConfigureServices(services =>
                {
                    services.AddMemoryCache();
                    services.AddJwksManager().PersistKeysInMemory();
                })
                .Configure(app =>
                {
                    app.UseDeveloperExceptionPage();
                    app.UseJwksDiscovery();
                    app.Map(new PathString("/auth"), x => x.UseMiddleware<AuthMiddleware>());

                })
                .UseUrls(ServerUrl).Build();
            TestServer.Start();

        }

        public void Dispose()
        {
            Task.WaitAll(TestServer?.StopAsync()!);
            HttpClient?.Dispose();
        }
    }
}
