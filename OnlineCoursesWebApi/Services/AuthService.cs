using OnlineCoursesWebApi.Configuration;
using OnlineCoursesWebApi.Helpers;
using OnlineCoursesWebApi.Interfaces.IServices;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace OnlineCoursesWebApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMemoryCache _cache;
        private readonly AuthHelper _authHelper;
        private const string TokenCacheKey = "AuthToken";

        public AuthService(IMemoryCache cache, HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _cache = cache;
            _authHelper = new AuthHelper(httpClient, apiSettings.Value.ApiUrl, apiSettings.Value.ApiKey);
        }

        public async Task<string> GetAuthTokenAsync()
        {
            var token = await _cache.GetOrCreateAsync(TokenCacheKey, async entry =>
            {
                string newToken = await _authHelper.GetTokenAsync();

                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(19));

                return newToken;
            });

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Failed to obtain a valid token.");
            }

            return token;
        }
    }

}
