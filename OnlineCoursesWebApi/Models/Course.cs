namespace OnlineCoursesWebApi.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<CourseDate> Dates { get; set; } = new List<CourseDate>();
    }
}
