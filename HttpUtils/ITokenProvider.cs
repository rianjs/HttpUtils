using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;

namespace HttpUtils
{
    /// <summary>
    /// The interface to implement for fetching tokens from the token provider origin. Do not implement caching in the derived object.
    /// </summary>
    public interface ITokenProvider
    {
        /// <summary>
        /// Gets the token from the token provider.
        /// </summary>
        Task<JwtSecurityToken> GetTokenAsync(CancellationToken ct);
    }
}