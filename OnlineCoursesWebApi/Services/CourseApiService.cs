using OnlineCoursesWebApi.Configuration;
using OnlineCoursesWebApi.DTOs;
using OnlineCoursesWebApi.Interfaces.IConfiguration;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Web;

namespace OnlineCoursesWebApi.Services
{
    public class CourseApiService : ICourseApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public CourseApiService(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _apiUrl = apiSettings.Value.ApiUrl;
        }

        public async Task<IEnumerable<ExternalCourseDTO>> FetchAllCoursesAsync(string token)
        {
            var courseDTOs = new List<ExternalCourseDTO>();
            string? nextPageLink = null;
            do
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var requestUri = nextPageLink ?? $"{_apiUrl}/courses";
                var uriBuilder = new UriBuilder(requestUri);
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["isDataUpdated"] = "true";
                uriBuilder.Query = query.ToString();

                var response = await _httpClient.GetAsync(uriBuilder.Uri);

                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException("Failed to fetch courses.");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var coursesResponse = JsonSerializer.Deserialize<CoursesApiResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (coursesResponse?.Data != null)
                {
                    courseDTOs.AddRange(coursesResponse.Data);
                }

                nextPageLink = coursesResponse?.NextPageLink;
            } while (!string.IsNullOrEmpty(nextPageLink));

            return courseDTOs;
        }
    }
}
