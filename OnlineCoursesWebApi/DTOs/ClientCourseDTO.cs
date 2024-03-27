namespace OnlineCoursesWebApi.DTOs
{
    public class ClientCourseDTO
    {
        public int CourseId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<DateTime> Dates { get; set; } = new List<DateTime>();
    }
}
