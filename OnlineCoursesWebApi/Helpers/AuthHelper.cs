using System.Text.Json;
using System.Text;
using OnlineCoursesWebApi.DTOs;

namespace OnlineCoursesWebApi.Helpers
{
    public class AuthHelper
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly string _apiKey;

        public AuthHelper(HttpClient httpClient, string apiUrl, string apiKey)
        {
            _httpClient = httpClient;
            _apiUrl = apiUrl;
            _apiKey = apiKey;
        }

        public async Task<string> GetTokenAsync()
        {
            var requestContent = new StringContent(JsonSerializer.Serialize(new { apiKey = _apiKey }), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_apiUrl + "/auth", requestContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Failed to authenticate.");
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent, options);

            if (tokenResponse == null || tokenResponse.Data == null || string.IsNullOrEmpty(tokenResponse.Data.AccessToken))
            {
                throw new InvalidOperationException("Token not found.");
            }

            return tokenResponse.Data.AccessToken;
        }
    }
}
