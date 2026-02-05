using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetDevPack.Security.JwtExtensions.Tests.Infra
{
    public class Server : IDisposable
    {
        public string ServerUrl { get; }
        public readonly IWebHost TestServer;
        public readonly HttpClient HttpClient;

        public Server()
        {
            var port = GetFreePort();
            ServerUrl = $"http://127.0.0.1:{port}";

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

            HttpClient = new HttpClient { BaseAddress = new Uri(ServerUrl) };
        }

        private static int GetFreePort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }

        public void Dispose()
        {
            Task.WaitAll(TestServer?.StopAsync()!);
            HttpClient?.Dispose();
        }
    }
}
