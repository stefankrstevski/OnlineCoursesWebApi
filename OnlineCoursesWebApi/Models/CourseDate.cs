namespace OnlineCoursesWebApi.Models
{
    public class CourseDate
    {
        public int CourseDateId { get; set; }
        public DateTime Date { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; } = new Course();
    }
}
