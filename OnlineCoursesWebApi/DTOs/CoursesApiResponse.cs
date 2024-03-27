using System.Text.Json.Serialization;

namespace OnlineCoursesWebApi.DTOs
{
    public class CoursesApiResponse
    {
        [JsonPropertyName("next_page_link")]
        public string NextPageLinkRaw { get; set; } = string.Empty;
        [JsonIgnore]
        public string NextPageLink => // Parse the raw link to extract the URL
    System.Text.RegularExpressions.Regex.Match(NextPageLinkRaw, "(?<=<).+?(?=>)").Value;
        [JsonPropertyName("total_count")]
        public int TotalCount { get; set; }
        [JsonPropertyName("max_limit")]
        public int MaxLimit { get; set; }
        [JsonPropertyName("data")]
        public List<ExternalCourseDTO> Data { get; set; } = new List<ExternalCourseDTO>();
    }
}
