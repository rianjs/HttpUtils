using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace HttpUtils
{
    /// <summary>
    /// Caching HttpHandler for Bearer tokens
    /// </summary>
    public class BearerTokenCachingHandler :
        DelegatingHandler
    {
        private readonly ITokenCache _tokenCache;

        public BearerTokenCachingHandler(
            HttpMessageHandler innerHandler,
            ITokenCache tokenCache) 
                : base(innerHandler)
        {
            _tokenCache = tokenCache;
        }

        private async Task<JwtSecurityToken> GetTokenAsync(CancellationToken ct)
        {
            var token = await _tokenCache.GetTokenAsync(DateTime.UtcNow, ct);
            return token;
        }

        /// <inheritdoc cref="DelegatingHandler"/>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _tokenCache.GetTokenAsync(DateTime.UtcNow, cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.ToString());
            return await base.SendAsync(request, cancellationToken);
        }
    }
}