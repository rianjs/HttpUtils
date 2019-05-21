using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HttpUtils;

namespace Tester
{
    public class NpEnvironmentTokenProvider :
        ITokenProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _endpoint;

        public NpEnvironmentTokenProvider(
            HttpClient npHttpTokenClient,
            string clientId,
            string clientSecret,
            string grantType,
            string scope)
        {
            _httpClient = npHttpTokenClient;
            _endpoint = $"/as/token.oauth2?grant_type={grantType}&client_id={clientId}&client_secret={clientSecret}&scope={scope}";
        }
        
        public async Task<JwtSecurityToken> GetTokenAsync(CancellationToken ct)
        {
            using (var response = await _httpClient.PostAsync(_endpoint, content: null))
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                
                var contents = await response.Content.ReadAsStringAsync();
                return jwtHandler.ReadJwtToken(contents);
            }
        }
    }
}