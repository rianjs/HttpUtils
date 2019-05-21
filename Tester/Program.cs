using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HttpUtils;

namespace Tester
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var ct = new CancellationToken();
            var npHttpClient = new HttpClient
            {
                BaseAddress = new Uri("https://tokens.example.com:9031"),
            };
            
            var tokenProvider = new NpEnvironmentTokenProvider(
                npHttpTokenClient: npHttpClient,
                clientId: "some-service-client",
                clientSecret: "some-client-secret",
                grantType: "client_credentials",
                scope: "some_api_client");
            
            var tokenCache = new TokenCache(tokenProvider, buffer: TimeSpan.FromSeconds(20), ct);
            var token = await tokenCache.GetTokenAsync(DateTime.UtcNow, ct);
            var npTokenCachingHandler = new BearerTokenCachingHandler(new HttpClientHandler(), tokenCache);
            
            
            var serviceHubClient = new HttpClient(npTokenCachingHandler)
            {
                BaseAddress = new Uri("https://webservice.example.com"),
            };
        }
    }
}