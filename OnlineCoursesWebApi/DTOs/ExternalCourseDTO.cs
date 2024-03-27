using System.Text.Json.Serialization;

namespace OnlineCoursesWebApi.DTOs
{
    public class ExternalCourseDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("course_name")]
        public string CourseName { get; set; } = string.Empty;
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }
    }
}
