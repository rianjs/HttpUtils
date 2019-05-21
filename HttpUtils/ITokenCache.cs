using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;

namespace HttpUtils
{
    /// <summary>
    /// The token cache interface
    /// </summary>
    public interface ITokenCache
    {
        /// <summary>
        /// Gets the token out of the cache, or refreshes the cache if the cached token is non-existent or expired.
        /// </summary>
        /// <param name="currentUtcTime">The current UTC time. Typically the caller passes in DateTime.UtcNow</param>
        /// <param name="ct"></param>
        /// <exception cref="ArgumentException">currentUtcTime must have DateTimeKind.Utc</exception>
        Task<JwtSecurityToken> GetTokenAsync(DateTime currentUtcTime, CancellationToken ct);
    }
}