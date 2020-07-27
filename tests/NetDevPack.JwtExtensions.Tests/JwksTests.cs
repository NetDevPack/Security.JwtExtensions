using ApiTest;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NetDevPack.JwtExtensions.Tests.Infra;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace NetDevPack.JwtExtensions.Tests
{
    public class JwksTests : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
        private readonly WebApplicationFactory<Startup> _webApplicationFactory;
        private readonly ITestOutputHelper _output;
        private HttpClient _client;
        private Server _server;

        private const string InvalidSignature = "eyJhbGciOiJFUzI1NiIsImtpZCI6Im1qeEQyQUphcXE4cW8zMWRiMjFyTGciLCJ0eXAiOiJhdCtqd3QifQ.eyJuYmYiOjE1OTU3MzQ0MzksImV4cCI6MTU5NTczODAzOSwiaXNzIjoiaHR0cHM6Ly9zc28uanBwcm9qZWN0Lm5ldCIsImF1ZCI6ImpwX2FwaSIsImNsaWVudF9pZCI6IklTNC1BZG1pbiIsInN1YiI6Ijc0MkYyMzk2LTg1N0MtNEI0Qy01NUExLTA4RDYzMTczMUIzRCIsImF1dGhfdGltZSI6MTU5NTczNDQzMiwiaWRwIjoiR29vZ2xlIiwiaXM0LXJpZ2h0cyI6Im1hbmFnZXIiLCJyb2xlIjoiQWRtaW5pc3RyYXRvciIsImVtYWlsIjoiYmhkZWJyaXRvQGdtYWlsLmNvbSIsInVzZXJuYW1lIjoiYnJ1bm8iLCJzY29wZSI6WyJvcGVuaWQiLCJwcm9maWxlIiwiZW1haWwiLCJyb2xlIiwianBfYXBpLmlzNCJdLCJhbXIiOlsiZXh0ZXJuYWwiXX0.i4_sfpzcS5oVVdgQqyH2qJfp02Jl-dAm3KuDr8G1BEz2flw6d3yPB8C82qhuM9PDNO7UBcXm8LJdMfI--SUK0A";

        public JwksTests(WebApplicationFactory<Startup> webApplicationFactory, ITestOutputHelper output)
        {
            _webApplicationFactory = webApplicationFactory;
            _output = output;
            _client = webApplicationFactory.CreateClient();
            _server = new Server();
        }

        [Fact]
        public async Task Should_200()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost/WeatherForecast");

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_401()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost/ProtectedWeatherForecast");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "generic token");
            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_Not_Validate_Bearer_Token_With_Invalid_Signature()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost/ProtectedWeatherForecast");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", InvalidSignature);
            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_Validate_Bearer_Token()
        {
            var tokenRequest = new HttpRequestMessage(HttpMethod.Get, "https://localhost:5001/auth");
            var tokenResponse = await _server.HttpClient.SendAsync(tokenRequest);
            var token = await tokenResponse.Content.ReadAsStringAsync();

            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost/ProtectedWeatherForecast");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(request);

            try { response.EnsureSuccessStatusCode(); } catch { _output.WriteLine(string.Join(" ", response.Headers.GetValues("WWW-Authenticate"))); throw; };
        }

        [Fact]
        public async Task Should_Get_Jwks_From_Cache()
        {
            var tokenRequest = new HttpRequestMessage(HttpMethod.Get, "https://localhost:5001/auth");
            var tokenResponse = await _server.HttpClient.SendAsync(tokenRequest);
            var token = await tokenResponse.Content.ReadAsStringAsync();

            // First Get
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost/ProtectedWeatherForecast");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(request);

            try { response.EnsureSuccessStatusCode(); } catch { _output.WriteLine(string.Join(" ", response.Headers.GetValues("WWW-Authenticate"))); throw; };

            // 2nd Get
            request = new HttpRequestMessage(HttpMethod.Get, "https://localhost/ProtectedWeatherForecast");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            response = await _client.SendAsync(request);

            try { response.EnsureSuccessStatusCode(); } catch { _output.WriteLine(string.Join(" ", response.Headers.GetValues("WWW-Authenticate"))); throw; };


            ServiceDiscoveryMiddleware.CallTimes.Should().BeLessOrEqualTo(1);
        }


        public void Dispose()
        {
            _server.Dispose();
        }
    }
}
