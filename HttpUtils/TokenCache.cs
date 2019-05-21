using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;

namespace HttpUtils
{
    /// <summary>
    /// <inheritdoc cref="ITokenCache"/>
    /// </summary>
    public class TokenCache :
        ITokenCache
    {
        private readonly ITokenProvider _tokenProvider;
        private readonly TimeSpan _buffer;
        private readonly SemaphoreSlim _semaphore;
        
        public TokenCache(
            ITokenProvider tokenProvider,
            TimeSpan buffer,
            CancellationToken ct)
        {
            if (buffer < TimeSpan.Zero)
            {
                throw new ArgumentException("Buffer must be a positive value");
            }
            
            _tokenProvider = tokenProvider;
            _buffer = buffer;
            _semaphore = new SemaphoreSlim(1, 1);
        }
        
        private JwtSecurityToken _token;
        /// <inheritdoc cref="ITokenCache"/>
        public async Task<JwtSecurityToken> GetTokenAsync(DateTime currentUtcTime, CancellationToken ct)
        {
            if (currentUtcTime.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException($"{nameof(currentUtcTime)} must be a UTC time");
            }
            
            try
            {
                await _semaphore.WaitAsync(ct);

                var isValid = _token != null && IsUsableToken(_token.ValidTo, currentUtcTime, _buffer);
                if (isValid)
                {
                    return _token;
                }
                
                _token = await _tokenProvider.GetTokenAsync(ct);
                return _token;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <summary>
        /// Examines the token's expiration time relative to the current moment and the specified safety buffer.
        /// </summary>
        /// <param name="validToUtc">The token's expiration time</param>
        /// <param name="comparisonTime">The current moment in UTC</param>
        /// <param name="buffer">The safety buffer</param>
        /// <exception cref="ArgumentException">currentUtcTime must have DateTimeKind.Utc</exception>
        public static bool IsUsableToken(DateTime validToUtc, DateTime comparisonTime, TimeSpan buffer)
        {
            if (comparisonTime.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException($"{nameof(comparisonTime)} must be a UTC time");
            }
            
            var validUntil = validToUtc.Subtract(buffer);
            return comparisonTime < validUntil;
        }
    }
}